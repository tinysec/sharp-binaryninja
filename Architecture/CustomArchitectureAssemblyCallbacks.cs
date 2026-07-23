using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		private delegate bool ManagedPatchCallback(byte[] data, ulong address);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool CanAssembleCallback(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool AssembleCallback(
			IntPtr context,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string code,
			ulong address,
			IntPtr result,
			IntPtr errors);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool PatchCallback(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.I1)]
		private delegate bool SkipAndReturnValueCallback(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length,
			ulong value);

		private void AddAssemblyAndPatchCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.canAssemble = UnsafeUtils.PinCallback<CanAssembleCallback>(
				this.CanAssembleAdapter);
			callbacks.assemble = UnsafeUtils.PinCallback<AssembleCallback>(this.AssembleAdapter);
			callbacks.isNeverBranchPatchAvailable = UnsafeUtils.PinCallback<PatchCallback>(
				this.IsNeverBranchPatchAvailableAdapter);
			callbacks.isAlwaysBranchPatchAvailable = UnsafeUtils.PinCallback<PatchCallback>(
				this.IsAlwaysBranchPatchAvailableAdapter);
			callbacks.isInvertBranchPatchAvailable = UnsafeUtils.PinCallback<PatchCallback>(
				this.IsInvertBranchPatchAvailableAdapter);
			callbacks.isSkipAndReturnZeroPatchAvailable = UnsafeUtils.PinCallback<PatchCallback>(
				this.IsSkipAndReturnZeroPatchAvailableAdapter);
			callbacks.isSkipAndReturnValuePatchAvailable = UnsafeUtils.PinCallback<PatchCallback>(
				this.IsSkipAndReturnValuePatchAvailableAdapter);
			callbacks.convertToNop = UnsafeUtils.PinCallback<PatchCallback>(
				this.ConvertToNopAdapter);
			callbacks.alwaysBranch = UnsafeUtils.PinCallback<PatchCallback>(
				this.AlwaysBranchAdapter);
			callbacks.invertBranch = UnsafeUtils.PinCallback<PatchCallback>(
				this.InvertBranchAdapter);
			callbacks.skipAndReturnValue = UnsafeUtils.PinCallback<SkipAndReturnValueCallback>(
				this.SkipAndReturnValueAdapter);
		}

		private bool CanAssembleAdapter(IntPtr context)
		{
			try
			{
				return this.CanAssemble();
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in CustomArchitecture.CanAssemble: {0}", exception);

				return false;
			}
		}

		private bool AssembleAdapter(
			IntPtr context,
			string code,
			ulong address,
			IntPtr result,
			IntPtr errors)
		{
			try
			{
				byte[] data = this.Assemble(code, address) ?? Array.Empty<byte>();
				NativeMethods.BNSetDataBufferContents(result, data, (ulong)data.Length);
				Marshal.WriteIntPtr(errors, NativeMethods.BNAllocString(string.Empty));

				return true;
			}
			catch (Exception exception)
			{
				Core.LogError("Unhandled exception in CustomArchitecture.Assemble: {0}", exception);
				NativeMethods.BNSetDataBufferContents(result, Array.Empty<byte>(), 0);
				Marshal.WriteIntPtr(errors, NativeMethods.BNAllocString(exception.Message));

				return false;
			}
		}

		private bool IsNeverBranchPatchAvailableAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatchAvailability(
				data,
				address,
				length,
				this.IsNeverBranchPatchAvailable,
				"IsNeverBranchPatchAvailable");
		}

		private bool IsAlwaysBranchPatchAvailableAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatchAvailability(
				data,
				address,
				length,
				this.IsAlwaysBranchPatchAvailable,
				"IsAlwaysBranchPatchAvailable");
		}

		private bool IsInvertBranchPatchAvailableAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatchAvailability(
				data,
				address,
				length,
				this.IsInvertBranchPatchAvailable,
				"IsInvertBranchPatchAvailable");
		}

		private bool IsSkipAndReturnZeroPatchAvailableAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatchAvailability(
				data,
				address,
				length,
				this.IsSkipAndReturnZeroPatchAvailable,
				"IsSkipAndReturnZeroPatchAvailable");
		}

		private bool IsSkipAndReturnValuePatchAvailableAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatchAvailability(
				data,
				address,
				length,
				this.IsSkipAndReturnValuePatchAvailable,
				"IsSkipAndReturnValuePatchAvailable");
		}

		private bool InvokePatchAvailability(
			IntPtr data,
			ulong address,
			ulong length,
			ManagedPatchCallback callback,
			string callbackName)
		{
			try
			{
				return callback(this.ReadPatchData(data, length), address);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.{0}: {1}",
					callbackName,
					exception);

				return false;
			}
		}

		private bool ConvertToNopAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatch(
				data,
				address,
				length,
				this.ConvertToNop,
				"ConvertToNop");
		}

		private bool AlwaysBranchAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatch(
				data,
				address,
				length,
				this.AlwaysBranch,
				"AlwaysBranch");
		}

		private bool InvertBranchAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length)
		{
			return this.InvokePatch(
				data,
				address,
				length,
				this.InvertBranch,
				"InvertBranch");
		}

		private bool InvokePatch(
			IntPtr data,
			ulong address,
			ulong length,
			ManagedPatchCallback callback,
			string callbackName)
		{
			try
			{
				byte[] managedData = this.ReadPatchData(data, length);
				bool result = callback(managedData, address);
				if (result && 0 != managedData.Length)
				{
					Marshal.Copy(managedData, 0, data, managedData.Length);
				}

				return result;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.{0}: {1}",
					callbackName,
					exception);

				return false;
			}
		}

		private bool SkipAndReturnValueAdapter(
			IntPtr context,
			IntPtr data,
			ulong address,
			ulong length,
			ulong value)
		{
			try
			{
				byte[] managedData = this.ReadPatchData(data, length);
				bool result = this.SkipAndReturnValue(managedData, address, value);
				if (result && 0 != managedData.Length)
				{
					Marshal.Copy(managedData, 0, data, managedData.Length);
				}

				return result;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.SkipAndReturnValue: {0}",
					exception);

				return false;
			}
		}

		private byte[] ReadPatchData(IntPtr data, ulong length)
		{
			byte[] result = new byte[checked((int)length)];
			if (0 != result.Length)
			{
				Marshal.Copy(data, result, 0, result.Length);
			}

			return result;
		}
	}
}
