using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed partial class LowLevelILFunction
	{
		public LowLevelILExpressionIndex AddRegisterOrFlagList(FlagOrRegister[] values)
		{
			List<ulong> operands = new List<ulong>(values.Length);

			foreach (FlagOrRegister value in values)
			{
				operands.Add(value.Identifier);
			}

			return this.AddOperandList(operands.ToArray());
		}

		public LowLevelILExpressionIndex EmitSetFlag(
			FlagIndex flag,
			LowLevelILExpressionIndex value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SET_FLAG,
				location,
				0,
				0,
				(ulong)flag,
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitSetRegisterStackTopRelative(
			ulong size,
			RegisterStackIndex registerStack,
			LowLevelILExpressionIndex entry,
			LowLevelILExpressionIndex value,
			FlagIndex? flag = null,
			SourceLocation? location = null)
		{
			return this.EmitSetRegisterStackRelative(
				size,
				registerStack,
				entry,
				value,
				flag,
				location);
		}

		public LowLevelILExpressionIndex EmitRegisterStackPush(
			ulong size,
			RegisterStackIndex registerStack,
			LowLevelILExpressionIndex value,
			FlagIndex? flag = null,
			SourceLocation? location = null)
		{
			return this.EmitSetRegisterStackPush(size, registerStack, value, flag, location);
		}

		public LowLevelILExpressionIndex EmitForceVer(
			ulong size,
			RegisterIndex register,
			SourceLocation? location = null)
		{
			return this.EmitForceVersion(size, register, location);
		}

		public LowLevelILExpressionIndex EmitFlag(
			FlagIndex flag,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_FLAG,
				location,
				0,
				0,
				(ulong)flag);
		}

		public LowLevelILExpressionIndex EmitRegisterStackTopRelative(
			ulong size,
			RegisterStackIndex registerStack,
			LowLevelILExpressionIndex entry,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_REL,
				location,
				size,
				0,
				(ulong)registerStack,
				(ulong)entry);
		}

		public LowLevelILExpressionIndex EmitRegisterStackFreeReg(
			RegisterIndex register,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_FREE_REG,
				location,
				0,
				0,
				(ulong)register);
		}

		public LowLevelILExpressionIndex EmitRegisterStackFreeTopRelative(
			RegisterStackIndex registerStack,
			LowLevelILExpressionIndex entry,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_FREE_REL,
				location,
				0,
				0,
				(ulong)registerStack,
				(ulong)entry);
		}

		public LowLevelILExpressionIndex EmitAddCarry(
			ulong size,
			LowLevelILExpressionIndex left,
			LowLevelILExpressionIndex right,
			LowLevelILExpressionIndex carry,
			ILFlag? flag = null,
			SourceLocation? location = null)
		{
			return this.EmitAddCarray(size, left, right, carry, flag, location);
		}

		public LowLevelILExpressionIndex EmitShiftLeft(
			ulong size,
			LowLevelILExpressionIndex left,
			LowLevelILExpressionIndex right,
			ILFlag? flag = null,
			SourceLocation? location = null)
		{
			return this.EmitLogicalShiftLeft(size, left, right, flag, location);
		}

		public LowLevelILExpressionIndex EmitRotateLeftCarry(
			ulong size,
			LowLevelILExpressionIndex left,
			LowLevelILExpressionIndex right,
			LowLevelILExpressionIndex carry,
			ILFlag? flag = null,
			SourceLocation? location = null)
		{
			return this.EmitRotateLeftCarray(size, left, right, carry, flag, location);
		}

		public LowLevelILExpressionIndex EmitRotateRightCarry(
			ulong size,
			LowLevelILExpressionIndex left,
			LowLevelILExpressionIndex right,
			LowLevelILExpressionIndex carry,
			ILFlag? flag = null,
			SourceLocation? location = null)
		{
			return this.EmitRotateRightCarray(size, left, right, carry, flag, location);
		}

		public LowLevelILExpressionIndex EmitAddOverflow(
			ulong size,
			LowLevelILExpressionIndex left,
			LowLevelILExpressionIndex right,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_ADD_OVERFLOW,
				location,
				size,
				(uint)left,
				(ulong)right);
		}

		public LowLevelILExpressionIndex EmitSystemCall(SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SYSCALL,
				location,
				0,
				0);
		}

		public LowLevelILExpressionIndex EmitGoto(
			LowLevelILLabel label,
			SourceLocation? location = null)
		{
			return this.EmitGoto(0, label, location);
		}

		public LowLevelILExpressionIndex EmitIf(
			LowLevelILExpressionIndex condition,
			LowLevelILLabel trueBranch,
			LowLevelILLabel falseBranch,
			SourceLocation? location = null)
		{
			return this.EmitIf(0, condition, trueBranch, falseBranch, location);
		}

		public LowLevelILExpressionIndex EmitIntrinsic(
			FlagOrRegister[] output,
			IntrinsicIndex intrinsic,
			LowLevelILExpressionIndex[] parameters,
			FlagIndex? flag = null,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_INTRINSIC,
				location,
				0,
				null == flag ? 0 : (uint)flag,
				(ulong)output.Length,
				(ulong)this.AddRegisterOrFlagList(output),
				(ulong)intrinsic,
				(ulong)this.EmitCallParameter(parameters, location));
		}

		public LowLevelILExpressionIndex EmitBreakpoint(SourceLocation? location = null)
		{
			return this.EmitBreakPoint(location);
		}

		public LowLevelILExpressionIndex EmitReturn(
			LowLevelILExpressionIndex destination,
			SourceLocation? location = null)
		{
			return this.EmitRet(destination, location);
		}

		public LowLevelILExpressionIndex EmitNoReturn(SourceLocation? location = null)
		{
			return this.EmitNoRet(location);
		}

		public LowLevelILExpressionIndex EmitUnimplementedMemoryRef(
			ulong size,
			LowLevelILExpressionIndex address,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_UNIMPL_MEM,
				location,
				size,
				0,
				(ulong)address);
		}
	}
}
