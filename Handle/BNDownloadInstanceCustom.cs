using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class DownloadInstance
	{
		private static readonly object registrationLock = new object();

		private static readonly List<DownloadInstance> registeredInstances =
			new List<DownloadInstance>();

		private readonly object responseLock = new object();

		private readonly Dictionary<IntPtr, ScopedAllocator> pendingResponses =
			new Dictionary<IntPtr, ScopedAllocator>();

		private bool initialReferencePending;

		private NativeDelegates.BNDownloadInstanceDestroy? destroyCallback;

		private NativeDelegates.BNDownloadInstancePerformRequest? performRequestCallback;

		private NativeDelegates.BNDownloadInstancePerformCustomRequest?
			performCustomRequestCallback;

		private NativeDelegates.BNDownloadInstanceFreeResponse? freeResponseCallback;

		/// <summary>Creates a custom instance owned by the supplied provider.</summary>
		protected DownloadInstance(DownloadProvider provider)
			: base(false)
		{
			if (null == provider)
			{
				throw new ArgumentNullException(nameof(provider));
			}

			this.destroyCallback = new NativeDelegates.BNDownloadInstanceDestroy(
				this.InvokeDestroy
			);
			this.performRequestCallback =
				new NativeDelegates.BNDownloadInstancePerformRequest(
					this.InvokePerformRequest
				);
			this.performCustomRequestCallback =
				new NativeDelegates.BNDownloadInstancePerformCustomRequest(
					this.InvokePerformCustomRequest
				);
			this.freeResponseCallback =
				new NativeDelegates.BNDownloadInstanceFreeResponse(
					this.InvokeFreeResponse
				);

			BNDownloadInstanceCallbacks callbacks = new BNDownloadInstanceCallbacks();
			callbacks.context = IntPtr.Zero;
			callbacks.destroyInstance = Marshal.GetFunctionPointerForDelegate(
				this.destroyCallback
			);
			callbacks.performRequest = Marshal.GetFunctionPointerForDelegate(
				this.performRequestCallback
			);
			callbacks.performCustomRequest = Marshal.GetFunctionPointerForDelegate(
				this.performCustomRequestCallback
			);
			callbacks.freeResponse = Marshal.GetFunctionPointerForDelegate(
				this.freeResponseCallback
			);

			IntPtr handle = NativeMethods.BNInitDownloadInstance(
				provider.DangerousGetHandle(),
				in callbacks
			);
			if (IntPtr.Zero == handle)
			{
				throw new InvalidOperationException(
					"The core rejected the download instance."
				);
			}

			this.SetHandle(handle);
			this.initialReferencePending = true;
			lock (DownloadInstance.registrationLock)
			{
				DownloadInstance.registeredInstances.Add(this);
			}
		}

		/// <summary>Handles a synchronous GET request from the core.</summary>
		protected abstract int PerformRequest(string url);

		/// <summary>Handles a synchronous custom request from the core.</summary>
		protected abstract int PerformCustomRequest(
			string method,
			string url,
			IReadOnlyDictionary<string, string> headers,
			out DownloadInstanceResponse? response
		);

		/// <summary>Releases resources owned by a custom instance.</summary>
		protected virtual void DestroyInstance()
		{
		}

		/// <summary>Reads the next request-body chunk into a buffer.</summary>
		protected unsafe long ReadData(byte[] buffer)
		{
			if (null == buffer)
			{
				throw new ArgumentNullException(nameof(buffer));
			}

			if (0 == buffer.Length)
			{
				return NativeMethods.BNReadDataForDownloadInstance(
					this.handle,
					IntPtr.Zero,
					0
				);
			}

			fixed (byte* data = buffer)
			{
				return NativeMethods.BNReadDataForDownloadInstance(
					this.handle,
					(IntPtr)data,
					(ulong)buffer.Length
				);
			}
		}

		/// <summary>Writes a response-body chunk to the active request.</summary>
		protected unsafe ulong WriteData(byte[] data)
		{
			if (null == data)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (0 == data.Length)
			{
				return NativeMethods.BNWriteDataForDownloadInstance(
					this.handle,
					IntPtr.Zero,
					0
				);
			}

			fixed (byte* buffer = data)
			{
				return NativeMethods.BNWriteDataForDownloadInstance(
					this.handle,
					(IntPtr)buffer,
					(ulong)data.Length
				);
			}
		}

		internal void ReleaseInitialReferenceForRegistration()
		{
			if (!this.initialReferencePending)
			{
				return;
			}

			this.initialReferencePending = false;
			NativeMethods.BNFreeDownloadInstance(this.handle);
		}

		private void InvokeDestroy(IntPtr context)
		{
			try
			{
				this.DestroyInstance();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in DownloadInstance.DestroyInstance: {0}",
					exception
				);
			}
			finally
			{
				this.DisposePendingResponses();
				lock (DownloadInstance.registrationLock)
				{
					DownloadInstance.registeredInstances.Remove(this);
				}

				this.SetHandleAsInvalid();
			}
		}

		private int InvokePerformRequest(IntPtr context, string url)
		{
			try
			{
				return this.PerformRequest(url);
			}
			catch (Exception exception)
			{
				this.Error = exception.GetType().Name;
				Core.LogError(
					"Unhandled exception in DownloadInstance.PerformRequest: {0}",
					exception
				);
				return -1;
			}
		}

		private int InvokePerformCustomRequest(
			IntPtr context,
			string method,
			string url,
			ulong headerCount,
			IntPtr headerKeys,
			IntPtr headerValues,
			IntPtr response
		)
		{
			Marshal.WriteIntPtr(response, IntPtr.Zero);
			try
			{
				string[] keys = UnsafeUtils.ReadUtf8StringArray(
					headerKeys,
					headerCount
				);
				string[] values = UnsafeUtils.ReadUtf8StringArray(
					headerValues,
					headerCount
				);
				Dictionary<string, string> headers =
					new Dictionary<string, string>();
				for (int i = 0; i < keys.Length; i++)
				{
					headers[keys[i]] = values[i];
				}

				int status = this.PerformCustomRequest(
					method,
					url,
					headers,
					out DownloadInstanceResponse? managedResponse
				);
				if (null != managedResponse)
				{
					Marshal.WriteIntPtr(
						response,
						this.AllocateResponse(managedResponse)
					);
				}

				return status;
			}
			catch (Exception exception)
			{
				this.Error = exception.GetType().Name;
				Core.LogError(
					"Unhandled exception in DownloadInstance.PerformCustomRequest: {0}",
					exception
				);
				return -1;
			}
		}

		private void InvokeFreeResponse(IntPtr context, IntPtr response)
		{
			if (IntPtr.Zero == response)
			{
				return;
			}

			ScopedAllocator? allocator = null;
			lock (this.responseLock)
			{
				if (this.pendingResponses.TryGetValue(response, out allocator))
				{
					this.pendingResponses.Remove(response);
				}
			}

			if (null == allocator)
			{
				Core.LogError(
					"DownloadInstance received an unknown response allocation."
				);
				return;
			}

			allocator.Dispose();
		}

		private IntPtr AllocateResponse(DownloadInstanceResponse response)
		{
			string[] keys = response.HeaderKeys ?? Array.Empty<string>();
			string[] values = response.HeaderValues ?? Array.Empty<string>();
			if (keys.Length != values.Length)
			{
				throw new ArgumentException(
					"Response header keys and values must have equal lengths.",
					nameof(response)
				);
			}

			for (int i = 0; i < keys.Length; i++)
			{
				if (null == keys[i] || null == values[i])
				{
					throw new ArgumentException(
						"Response headers cannot contain null keys or values.",
						nameof(response)
					);
				}
			}

			ScopedAllocator allocator = new ScopedAllocator();
			try
			{
				BNDownloadInstanceResponse native = new BNDownloadInstanceResponse();
				native.statusCode = response.StatusCode;
				native.headerCount = (ulong)keys.Length;
				native.headerKeys = allocator.AllocUtf8StringArray(keys);
				native.headerValues = allocator.AllocUtf8StringArray(values);
				IntPtr result = allocator.AllocStruct(native);

				lock (this.responseLock)
				{
					this.pendingResponses.Add(result, allocator);
				}

				return result;
			}
			catch
			{
				allocator.Dispose();
				throw;
			}
		}

		private void DisposePendingResponses()
		{
			ScopedAllocator[] allocators;
			lock (this.responseLock)
			{
				allocators = new ScopedAllocator[this.pendingResponses.Count];
				this.pendingResponses.Values.CopyTo(allocators, 0);
				this.pendingResponses.Clear();
			}

			foreach (ScopedAllocator allocator in allocators)
			{
				allocator.Dispose();
			}
		}
	}
}
