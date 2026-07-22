using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Supplies request bytes to a download provider.
	/// </summary>
	/// <param name="buffer">Buffer to fill with the next request-body chunk.</param>
	/// <returns>The number of bytes written, zero at end of input, or -1 on failure.</returns>
	public delegate long DownloadReadDelegate(byte[] buffer);

	/// <summary>
	/// Consumes response bytes produced by a download provider.
	/// </summary>
	/// <param name="data">Response-body chunk to consume.</param>
	/// <returns>The number of bytes consumed.</returns>
	public delegate ulong DownloadWriteDelegate(byte[] data);

	public abstract partial class DownloadInstance
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate long NativeReadCallback(IntPtr data, ulong length, IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate ulong NativeWriteCallback(IntPtr data, ulong length, IntPtr context);

		private sealed class RequestCallbackContext
		{
			private readonly DownloadReadDelegate? read;
			private readonly DownloadWriteDelegate write;
			private readonly ProgressDelegate? progress;
			private ExceptionDispatchInfo? exception;

			internal RequestCallbackContext(
				DownloadReadDelegate? read,
				DownloadWriteDelegate write,
				ProgressDelegate? progress)
			{
				this.read = read;
				this.write = write;
				this.progress = progress;
			}

			internal long Read(IntPtr data, ulong length, IntPtr context)
			{
				if (null != this.exception)
				{
					return -1;
				}

				if (null == this.read)
				{
					return 0;
				}

				try
				{
					int managedLength = checked((int)length);
					byte[] buffer = new byte[managedLength];
					long readLength = this.read(buffer);
					if (-1 > readLength || (long)managedLength < readLength)
					{
						throw new InvalidOperationException(
							"The download read callback returned an invalid byte count.");
					}

					if (0 < readLength)
					{
						Marshal.Copy(buffer, 0, data, checked((int)readLength));
					}

					return readLength;
				}
				catch (Exception caught)
				{
					this.Capture(caught);
					return -1;
				}
			}

			internal ulong Write(IntPtr data, ulong length, IntPtr context)
			{
				if (null != this.exception)
				{
					return 0;
				}

				try
				{
					int managedLength = checked((int)length);
					byte[] buffer = new byte[managedLength];
					if (0 < managedLength)
					{
						Marshal.Copy(data, buffer, 0, managedLength);
					}

					ulong writtenLength = this.write(buffer);
					if (length < writtenLength)
					{
						throw new InvalidOperationException(
							"The download write callback returned an invalid byte count.");
					}

					return writtenLength;
				}
				catch (Exception caught)
				{
					this.Capture(caught);
					return 0;
				}
			}

			internal bool Progress(IntPtr context, ulong current, ulong total)
			{
				if (null != this.exception)
				{
					return false;
				}

				if (null == this.progress)
				{
					return true;
				}

				try
				{
					return this.progress(current, total);
				}
				catch (Exception caught)
				{
					this.Capture(caught);
					return false;
				}
			}

			internal void ThrowIfFailed()
			{
				if (null != this.exception)
				{
					this.exception.Throw();
				}
			}

			private void Capture(Exception caught)
			{
				if (null == this.exception)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
				}
			}
		}

		/// <summary>
		/// Performs a synchronous GET request and streams response bytes to a callback.
		/// </summary>
		/// <param name="url">Request URL.</param>
		/// <param name="write">Callback that consumes response-body chunks.</param>
		/// <param name="progress">Optional transfer progress callback.</param>
		/// <returns>Zero on success, or the provider's nonzero failure status.</returns>
		public int PerformRequest(
			string url,
			DownloadWriteDelegate write,
			ProgressDelegate? progress = null)
		{
			if (null == url)
			{
				throw new ArgumentNullException(nameof(url));
			}

			if (null == write)
			{
				throw new ArgumentNullException(nameof(write));
			}

			RequestCallbackContext callbackContext =
				new RequestCallbackContext(null, write, progress);
			NativeWriteCallback nativeWrite = callbackContext.Write;
			NativeDelegates.BNProgressFunction nativeProgress = callbackContext.Progress;
			BNDownloadInstanceOutputCallbacks callbacks = new BNDownloadInstanceOutputCallbacks
			{
				writeCallback = Marshal.GetFunctionPointerForDelegate<NativeWriteCallback>(nativeWrite),
				writeContext = IntPtr.Zero,
				progressCallback = Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress),
				progressContext = IntPtr.Zero
			};
			int result;

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				try
				{
					result = NativeMethods.BNPerformDownloadRequest(
						this.handle,
						url,
						allocator.AllocStruct<BNDownloadInstanceOutputCallbacks>(callbacks));
				}
				finally
				{
					GC.KeepAlive(nativeWrite);
					GC.KeepAlive(nativeProgress);
				}
			}

			callbackContext.ThrowIfFailed();
			return result;
		}

		/// <summary>
		/// Performs a synchronous request with headers and streaming input/output callbacks.
		/// </summary>
		/// <param name="method">HTTP method.</param>
		/// <param name="url">Request URL.</param>
		/// <param name="headers">Optional request headers.</param>
		/// <param name="read">Optional callback supplying request-body chunks.</param>
		/// <param name="write">Callback consuming response-body chunks.</param>
		/// <param name="response">Receives response status and headers when provided by the core.</param>
		/// <param name="progress">Optional transfer progress callback.</param>
		/// <returns>Zero on success, or the provider's nonzero failure status.</returns>
		public int PerformCustomRequest(
			string method,
			string url,
			IReadOnlyDictionary<string, string>? headers,
			DownloadReadDelegate? read,
			DownloadWriteDelegate write,
			out DownloadInstanceResponse? response,
			ProgressDelegate? progress = null)
		{
			if (null == method)
			{
				throw new ArgumentNullException(nameof(method));
			}

			if (null == url)
			{
				throw new ArgumentNullException(nameof(url));
			}

			if (null == write)
			{
				throw new ArgumentNullException(nameof(write));
			}

			int headerCount = null == headers ? 0 : headers.Count;
			string[] headerKeys = new string[headerCount];
			string[] headerValues = new string[headerCount];
			if (null != headers)
			{
				int index = 0;
				foreach (KeyValuePair<string, string> header in headers)
				{
					if (null == header.Key || null == header.Value)
					{
						throw new ArgumentException(
							"Request headers cannot contain null keys or values.",
							nameof(headers));
					}

					headerKeys[index] = header.Key;
					headerValues[index] = header.Value;
					index++;
				}
			}

			RequestCallbackContext callbackContext =
				new RequestCallbackContext(read, write, progress);
			NativeReadCallback nativeRead = callbackContext.Read;
			NativeWriteCallback nativeWrite = callbackContext.Write;
			NativeDelegates.BNProgressFunction nativeProgress = callbackContext.Progress;
			BNDownloadInstanceInputOutputCallbacks callbacks =
				new BNDownloadInstanceInputOutputCallbacks
				{
					readCallback = Marshal.GetFunctionPointerForDelegate<NativeReadCallback>(nativeRead),
					readContext = IntPtr.Zero,
					writeCallback = Marshal.GetFunctionPointerForDelegate<NativeWriteCallback>(nativeWrite),
					writeContext = IntPtr.Zero,
					progressCallback = Marshal.GetFunctionPointerForDelegate<NativeDelegates.BNProgressFunction>(nativeProgress),
					progressContext = IntPtr.Zero
				};
			IntPtr nativeResponse = IntPtr.Zero;
			int result;

			using (ScopedAllocator allocator = new ScopedAllocator())
			{
				try
				{
					result = NativeMethods.BNPerformCustomRequest(
						this.handle,
						method,
						url,
						(ulong)headerCount,
						allocator.AllocUtf8StringArray(headerKeys),
						allocator.AllocUtf8StringArray(headerValues),
						out nativeResponse,
						allocator.AllocStruct<BNDownloadInstanceInputOutputCallbacks>(callbacks));
				}
				finally
				{
					GC.KeepAlive(nativeRead);
					GC.KeepAlive(nativeWrite);
					GC.KeepAlive(nativeProgress);
				}
			}

			try
			{
				response = IntPtr.Zero == nativeResponse
					? null
					: ReadResponse(nativeResponse);
			}
			finally
			{
				if (IntPtr.Zero != nativeResponse)
				{
					NativeMethods.BNFreeDownloadInstanceResponse(nativeResponse);
				}
			}

			callbackContext.ThrowIfFailed();
			return result;
		}

		private static DownloadInstanceResponse ReadResponse(IntPtr responsePointer)
		{
			BNDownloadInstanceResponse nativeResponse =
				Marshal.PtrToStructure<BNDownloadInstanceResponse>(responsePointer);
			string[] keys = UnsafeUtils.ReadUtf8StringArray(
				nativeResponse.headerKeys,
				nativeResponse.headerCount);
			string[] values = UnsafeUtils.ReadUtf8StringArray(
				nativeResponse.headerValues,
				nativeResponse.headerCount);

			return new DownloadInstanceResponse
			{
				StatusCode = nativeResponse.statusCode,
				HeaderCount = nativeResponse.headerCount,
				HeaderKeys = keys,
				HeaderValues = values
			};
		}
	}
}
