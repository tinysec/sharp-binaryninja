using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool GetInstructionInfoCallback(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong maximumLength,
			IntPtr result);

		private void AddInstructionCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.getInstructionInfo = UnsafeUtils.PinCallback<GetInstructionInfoCallback>(
				this.GetInstructionInfoAdapter);
		}

		private bool GetInstructionInfoAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong maximumLength,
			IntPtr result)
		{
			try
			{
				byte[] bytes = new byte[checked((int)maximumLength)];
				if (0 != bytes.Length)
				{
					Marshal.Copy(data, bytes, 0, bytes.Length);
				}

				InstructionInfo? info = this.GetInstructionInfo(bytes, address);
				if (null == info)
				{
					return false;
				}

				Marshal.StructureToPtr(info.ToNative(), result, false);

				return true;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetInstructionInfo: {0}",
					exception);

				return false;
			}
		}
	}
}
