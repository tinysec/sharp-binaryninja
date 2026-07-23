using System;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	public abstract partial class CustomArchitecture
	{
		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate ulong GetFlagWriteLowLevelILCallback(
			IntPtr context,
			LowLevelILOperation operation,
			ulong size,
			uint flagWriteType,
			uint flag,
			IntPtr operands,
			ulong operandCount,
			IntPtr il);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate ulong GetFlagConditionLowLevelILCallback(
			IntPtr context,
			LowLevelILFlagCondition condition,
			uint semanticClass,
			IntPtr il);

		[UnmanagedFunctionPointer(System.Runtime.InteropServices.CallingConvention.Cdecl)]
		private delegate ulong GetSemanticFlagGroupLowLevelILCallback(
			IntPtr context,
			uint semanticGroup,
			IntPtr il);

		private void AddFlagLoweringCallbacks(ref BNCustomArchitecture callbacks)
		{
			callbacks.getFlagWriteLowLevelIL =
				UnsafeUtils.PinCallback<GetFlagWriteLowLevelILCallback>(
					this.GetFlagWriteLowLevelILAdapter);
			callbacks.getFlagConditionLowLevelIL =
				UnsafeUtils.PinCallback<GetFlagConditionLowLevelILCallback>(
					this.GetFlagConditionLowLevelILAdapter);
			callbacks.getSemanticFlagGroupLowLevelIL =
				UnsafeUtils.PinCallback<GetSemanticFlagGroupLowLevelILCallback>(
					this.GetSemanticFlagGroupLowLevelILAdapter);
		}

		private ulong GetFlagWriteLowLevelILAdapter(
			IntPtr context,
			LowLevelILOperation operation,
			ulong size,
			uint flagWriteType,
			uint flag,
			IntPtr operands,
			ulong operandCount,
			IntPtr ilHandle)
		{
			try
			{
				RegisterOrConstant[] managedOperands = UnsafeUtils.ReadStructArray<
					BNRegisterOrConstant,
					RegisterOrConstant>(
					operands,
					operandCount,
					RegisterOrConstant.FromNative);

				using (LowLevelILFunction il = this.CreateCallbackLowLevelIL(ilHandle))
				{
					return this.GetFlagWriteLowLevelIL(
						operation,
						size,
						flagWriteType,
						flag,
						managedOperands,
						il);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetFlagWriteLowLevelIL: {0}",
					exception);

				return 0;
			}
		}

		private ulong GetFlagConditionLowLevelILAdapter(
			IntPtr context,
			LowLevelILFlagCondition condition,
			uint semanticClass,
			IntPtr ilHandle)
		{
			try
			{
				using (LowLevelILFunction il = this.CreateCallbackLowLevelIL(ilHandle))
				{
					return this.GetFlagConditionLowLevelIL(condition, semanticClass, il);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetFlagConditionLowLevelIL: {0}",
					exception);

				return 0;
			}
		}

		private ulong GetSemanticFlagGroupLowLevelILAdapter(
			IntPtr context,
			uint semanticGroup,
			IntPtr ilHandle)
		{
			try
			{
				using (LowLevelILFunction il = this.CreateCallbackLowLevelIL(ilHandle))
				{
					return this.GetSemanticFlagGroupLowLevelIL(semanticGroup, il);
				}
			}
			catch (Exception exception)
			{
				Core.LogError(
					"Unhandled exception in CustomArchitecture.GetSemanticFlagGroupLowLevelIL: {0}",
					exception);

				return 0;
			}
		}

		private LowLevelILFunction CreateCallbackLowLevelIL(IntPtr handle)
		{
			return LowLevelILFunction.MustNewFromHandle(
				handle,
				false,
				this.registeredArchitecture);
		}
	}
}
