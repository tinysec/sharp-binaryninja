using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr GetRegisterNameCallback(IntPtr context, uint register);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate IntPtr GetRegisterListCallback(IntPtr context, out ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void FreeRegisterListCallback(
			IntPtr context,
			IntPtr registers,
			ulong count);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void GetRegisterInfoCallback(
			IntPtr context,
			uint register,
			IntPtr result);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate uint GetRegisterIndexCallback(IntPtr context);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate void GetRegisterStackInfoCallback(
			IntPtr context,
			uint registerStack,
			IntPtr result);

		private void AddRegisterCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.getRegisterName = UnsafeUtils.PinCallback<GetRegisterNameCallback>(
				this.GetRegisterNameAdapter);
			callbacks.getFullWidthRegisters = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetFullWidthRegistersAdapter);
			callbacks.getAllRegisters = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetAllRegistersAdapter);
			callbacks.freeRegisterList = UnsafeUtils.PinCallback<FreeRegisterListCallback>(
				this.FreeRegisterListAdapter);
			callbacks.getRegisterInfo = UnsafeUtils.PinCallback<GetRegisterInfoCallback>(
				this.GetRegisterInfoAdapter);
			callbacks.getStackPointerRegister = UnsafeUtils.PinCallback<GetRegisterIndexCallback>(
				this.GetStackPointerRegisterAdapter);
			callbacks.getLinkRegister = UnsafeUtils.PinCallback<GetRegisterIndexCallback>(
				this.GetLinkRegisterAdapter);
			callbacks.getGlobalRegisters = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetGlobalRegistersAdapter);
			callbacks.getSystemRegisters = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetSystemRegistersAdapter);
			callbacks.getRegisterStackName = UnsafeUtils.PinCallback<GetRegisterNameCallback>(
				this.GetRegisterStackNameAdapter);
			callbacks.getAllRegisterStacks = UnsafeUtils.PinCallback<GetRegisterListCallback>(
				this.GetAllRegisterStacksAdapter);
			callbacks.getRegisterStackInfo =
				UnsafeUtils.PinCallback<GetRegisterStackInfoCallback>(
					this.GetRegisterStackInfoAdapter);
		}

		private IntPtr GetRegisterNameAdapter(IntPtr context, uint register)
		{
			try
			{
				return NativeMethods.BNAllocString(this.GetRegisterName((RegisterIndex)register));
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetRegisterName: {0}",
					exception);
				return NativeMethods.BNAllocString(string.Empty);
			}
		}

		private IntPtr GetFullWidthRegistersAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetFullWidthRegisters,
				"GetFullWidthRegisters",
				out count);
		}

		private IntPtr GetAllRegistersAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetAllRegisters,
				"GetAllRegisters",
				out count);
		}

		private IntPtr GetGlobalRegistersAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetGlobalRegisters,
				"GetGlobalRegisters",
				out count);
		}

		private IntPtr GetSystemRegistersAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetSystemRegisters,
				"GetSystemRegisters",
				out count);
		}

		private IntPtr GetAllRegisterStacksAdapter(IntPtr context, out ulong count)
		{
			return this.GetRegisterListAdapter(
				this.GetAllRegisterStacks,
				"GetAllRegisterStacks",
				out count);
		}

		private IntPtr GetRegisterListAdapter(
			Func<uint[]> callback,
			string callbackName,
			out ulong count)
		{
			try
			{
				uint[] registers = callback() ?? Array.Empty<uint>();
				return this.AllocateRegisterList(registers, out count);
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.{0}: {1}",
					callbackName,
					exception);
				count = 0;
				return IntPtr.Zero;
			}
		}

		private IntPtr AllocateRegisterList(uint[] registers, out ulong count)
		{
			count = (ulong)registers.Length;
			if (0 == registers.Length)
			{
				return IntPtr.Zero;
			}

			IntPtr result = Marshal.AllocHGlobal(
				checked(registers.Length * sizeof(uint)));
			for (int index = 0; index < registers.Length; index++)
			{
				Marshal.WriteInt32(
					result,
					checked(index * sizeof(uint)),
					unchecked((int)registers[index]));
			}

			return result;
		}

		private void FreeRegisterListAdapter(
			IntPtr context,
			IntPtr registers,
			ulong count)
		{
			if (IntPtr.Zero != registers)
			{
				Marshal.FreeHGlobal(registers);
			}
		}

		private void GetRegisterInfoAdapter(
			IntPtr context,
			uint register,
			IntPtr result)
		{
			BNRegisterInfo native = new BNRegisterInfo();
			try
			{
				RegisterInfo info = this.GetRegisterInfo(register);
				native.fullWidthRegister = info.FullWidthRegister;
				native.offset = info.Offset;
				native.size = info.Size;
				native.extend = info.Extend;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetRegisterInfo: {0}",
					exception);
			}

			Marshal.StructureToPtr(native, result, false);
		}

		private uint GetStackPointerRegisterAdapter(IntPtr context)
		{
			try
			{
				return this.GetStackPointerRegister();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetStackPointerRegister: {0}",
					exception);
				return 0;
			}
		}

		private uint GetLinkRegisterAdapter(IntPtr context)
		{
			try
			{
				return this.GetLinkRegister();
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetLinkRegister: {0}",
					exception);
				return 0;
			}
		}

		private IntPtr GetRegisterStackNameAdapter(IntPtr context, uint registerStack)
		{
			try
			{
				return NativeMethods.BNAllocString(this.GetRegisterStackName(registerStack));
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetRegisterStackName: {0}",
					exception);
				return NativeMethods.BNAllocString(string.Empty);
			}
		}

		private void GetRegisterStackInfoAdapter(
			IntPtr context,
			uint registerStack,
			IntPtr result)
		{
			BNRegisterStackInfo native = new BNRegisterStackInfo();
			try
			{
				RegisterStackInfo info = this.GetRegisterStackInfo(registerStack);
				native.firstStorageReg = info.FirstStorageReg;
				native.firstTopRelativeReg = info.FirstTopRelativeReg;
				native.storageCount = info.StorageCount;
				native.topRelativeCount = info.TopRelativeCount;
				native.stackTopReg = info.StackTopReg;
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetRegisterStackInfo: {0}",
					exception);
			}

			Marshal.StructureToPtr(native, result, false);
		}
	}
}
