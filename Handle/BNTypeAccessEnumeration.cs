using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// Receives a terminal type and field-resolution path found for a typed memory access.
	/// </summary>
	public delegate void TypeAccessCallback(
		TypeWithConfidence? type,
		FieldResolutionInfo path);

	public partial class Type
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void NativeTypeAccessCallback(
			IntPtr context,
			IntPtr type,
			IntPtr path);

		private sealed class TypeAccessContext
		{
			private readonly TypeAccessCallback callback;
			private ExceptionDispatchInfo? exception;

			internal TypeAccessContext(TypeAccessCallback callback)
			{
				this.callback = callback;
			}

			internal void Invoke(IntPtr context, IntPtr type, IntPtr path)
			{
				if (null != this.exception)
				{
					return;
				}

				try
				{
					BNTypeWithConfidence nativeType = Marshal.PtrToStructure<BNTypeWithConfidence>(type);
					TypeWithConfidence? managedType = null;
					if (IntPtr.Zero != nativeType.type)
					{
						managedType = TypeWithConfidence.FromNative(nativeType);
					}

					FieldResolutionInfo managedPath = FieldResolutionInfo.MustNewFromHandle(path);
					this.callback(managedType, managedPath);
				}
				catch (Exception caught)
				{
					this.exception = ExceptionDispatchInfo.Capture(caught);
				}
			}

			internal void ThrowIfFailed()
			{
				if (null != this.exception)
				{
					this.exception.Throw();
				}
			}
		}

		/// <summary>
		/// Enumerates terminal types that can satisfy an access through this type.
		/// </summary>
		/// <param name="data">Binary view used to resolve named types.</param>
		/// <param name="offset">Byte offset of the access within this type.</param>
		/// <param name="size">Size in bytes of the access.</param>
		/// <param name="baseConfidence">Confidence assigned to the starting type.</param>
		/// <param name="terminal">Callback receiving independently owned type and path wrappers.</param>
		/// <returns>True when the core successfully enumerated the access.</returns>
		public bool EnumerateTypesForAccess(
			BinaryView data,
			ulong offset,
			ulong size,
			byte baseConfidence,
			TypeAccessCallback terminal)
		{
			if (null == data)
			{
				throw new ArgumentNullException(nameof(data));
			}

			if (null == terminal)
			{
				throw new ArgumentNullException(nameof(terminal));
			}

			TypeAccessContext callbackContext = new TypeAccessContext(terminal);
			NativeTypeAccessCallback nativeCallback = callbackContext.Invoke;
			bool result;

			try
			{
				result = NativeMethods.BNEnumerateTypesForAccess(
					this.handle,
					data.DangerousGetHandle(),
					offset,
					size,
					baseConfidence,
					Marshal.GetFunctionPointerForDelegate<NativeTypeAccessCallback>(nativeCallback),
					IntPtr.Zero);
			}
			finally
			{
				GC.KeepAlive(nativeCallback);
			}

			callbackContext.ThrowIfFailed();
			return result;
		}
	}
}
