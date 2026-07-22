using System.Collections.Generic;

namespace BinaryNinja
{
	public sealed partial class LowLevelILFunction
	{
		public LowLevelILExpressionIndex AddIndexList(ulong[] indexes)
		{
			return this.AddOperandList(indexes);
		}

		public LowLevelILExpressionIndex AddSSARegisterList(
			LowLevelILSSARegister[] registers)
		{
			List<ulong> operands = new List<ulong>(registers.Length * 2);

			foreach (LowLevelILSSARegister register in registers)
			{
				operands.Add((ulong)register.Register.Index);
				operands.Add(register.Version);
			}

			return this.AddOperandList(operands.ToArray());
		}

		public LowLevelILExpressionIndex AddSSARegisterStackList(
			SSARegisterStack[] registerStacks)
		{
			List<ulong> operands = new List<ulong>(registerStacks.Length * 2);

			foreach (SSARegisterStack registerStack in registerStacks)
			{
				operands.Add((ulong)registerStack.RegisterStack.Index);
				operands.Add(registerStack.Version);
			}

			return this.AddOperandList(operands.ToArray());
		}

		public LowLevelILExpressionIndex AddSSAFlagList(LowLevelILSSAFlag[] flags)
		{
			List<ulong> operands = new List<ulong>(flags.Length * 2);

			foreach (LowLevelILSSAFlag flag in flags)
			{
				operands.Add((ulong)flag.Flag.Index);
				operands.Add(flag.Version);
			}

			return this.AddOperandList(operands.ToArray());
		}

		public LowLevelILExpressionIndex AddSSAFlagOrRegisterList(
			SSAFlagOrRegister[] values)
		{
			List<ulong> operands = new List<ulong>(values.Length * 2);

			foreach (SSAFlagOrRegister value in values)
			{
				operands.Add(value.FlagOrRegister.Identifier);
				operands.Add(value.Version);
			}

			return this.AddOperandList(operands.ToArray());
		}

		public LowLevelILExpressionIndex EmitSetRegisterSSA(
			ulong size,
			LowLevelILSSARegister register,
			LowLevelILExpressionIndex value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SET_REG_SSA,
				location,
				size,
				0,
				(ulong)register.Register.Index,
				register.Version,
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitSetRegisterSSAPartial(
			ulong size,
			LowLevelILSSARegister fullRegister,
			RegisterIndex partialRegister,
			LowLevelILExpressionIndex value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SET_REG_SSA_PARTIAL,
				location,
				size,
				0,
				(ulong)fullRegister.Register.Index,
				fullRegister.Version,
				(ulong)partialRegister,
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitRegisterSplitDestinationSSA(
			ulong size,
			LowLevelILSSARegister register,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_SPLIT_DEST_SSA,
				location,
				size,
				0,
				(ulong)register.Register.Index,
				register.Version);
		}

		public LowLevelILExpressionIndex EmitSetRegisterSplitSSA(
			ulong size,
			LowLevelILSSARegister high,
			LowLevelILSSARegister low,
			LowLevelILExpressionIndex value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SET_REG_SPLIT_SSA,
				location,
				size,
				0,
				(ulong)this.EmitRegisterSplitDestinationSSA(size, high, location),
				(ulong)this.EmitRegisterSplitDestinationSSA(size, low, location),
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitRegisterStackDestinationSSA(
			ulong size,
			RegisterStackIndex registerStack,
			ulong destinationVersion,
			ulong sourceVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_DEST_SSA,
				location,
				size,
				0,
				(ulong)registerStack,
				destinationVersion,
				sourceVersion);
		}

		public LowLevelILExpressionIndex EmitSetRegisterStackTopRelativeSSA(
			ulong size,
			RegisterStackIndex registerStack,
			ulong destinationVersion,
			ulong sourceVersion,
			LowLevelILExpressionIndex entry,
			LowLevelILSSARegister top,
			LowLevelILExpressionIndex value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SET_REG_STACK_REL_SSA,
				location,
				size,
				0,
				(ulong)this.EmitRegisterStackDestinationSSA(
					size,
					registerStack,
					destinationVersion,
					sourceVersion,
					location),
				(ulong)entry,
				(ulong)this.EmitRegisterSSA(0, top, location),
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitSetRegisterStackAbsoluteSSA(
			ulong size,
			RegisterStackIndex registerStack,
			ulong destinationVersion,
			ulong sourceVersion,
			RegisterIndex register,
			LowLevelILExpressionIndex value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SET_REG_STACK_ABS_SSA,
				location,
				size,
				0,
				(ulong)this.EmitRegisterStackDestinationSSA(
					size,
					registerStack,
					destinationVersion,
					sourceVersion,
					location),
				(ulong)register,
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitSetFlagSSA(
			LowLevelILSSAFlag flag,
			LowLevelILExpressionIndex value,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SET_FLAG_SSA,
				location,
				0,
				0,
				(ulong)flag.Flag.Index,
				flag.Version,
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitForceVersionSSA(
			ulong size,
			LowLevelILSSARegister destination,
			LowLevelILSSARegister source,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_FORCE_VER_SSA,
				location,
				size,
				0,
				(ulong)destination.Register.Index,
				destination.Version,
				(ulong)source.Register.Index,
				source.Version);
		}

		public LowLevelILExpressionIndex EmitForceVerSSA(
			ulong size,
			LowLevelILSSARegister destination,
			LowLevelILSSARegister source,
			SourceLocation? location = null)
		{
			return this.EmitForceVersionSSA(size, destination, source, location);
		}

		public LowLevelILExpressionIndex EmitAssertSSA(
			ulong size,
			LowLevelILSSARegister source,
			PossibleValueSet constraint,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_ASSERT_SSA,
				location,
				size,
				0,
				(ulong)source.Register.Index,
				source.Version,
				(ulong)this.CachePossibleValueSet(constraint));
		}

		public LowLevelILExpressionIndex EmitLoadSSA(
			ulong size,
			LowLevelILExpressionIndex address,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_LOAD_SSA,
				location,
				size,
				0,
				(ulong)address,
				sourceMemoryVersion);
		}

		public LowLevelILExpressionIndex EmitStoreSSA(
			ulong size,
			LowLevelILExpressionIndex address,
			LowLevelILExpressionIndex value,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_STORE_SSA,
				location,
				size,
				0,
				(ulong)address,
				destinationMemoryVersion,
				sourceMemoryVersion,
				(ulong)value);
		}

		public LowLevelILExpressionIndex EmitRegisterSSA(
			ulong size,
			LowLevelILSSARegister register,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_SSA,
				location,
				size,
				0,
				(ulong)register.Register.Index,
				register.Version);
		}

		public LowLevelILExpressionIndex EmitRegisterSSAPartial(
			ulong size,
			LowLevelILSSARegister fullRegister,
			RegisterIndex partialRegister,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_SSA_PARTIAL,
				location,
				size,
				0,
				(ulong)fullRegister.Register.Index,
				fullRegister.Version,
				(ulong)partialRegister);
		}

		public LowLevelILExpressionIndex EmitRegisterSplitSSA(
			ulong size,
			LowLevelILSSARegister high,
			LowLevelILSSARegister low,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_SPLIT_SSA,
				location,
				size,
				0,
				(ulong)high.Register.Index,
				high.Version,
				(ulong)low.Register.Index,
				low.Version);
		}

		public LowLevelILExpressionIndex EmitRegisterStackTopRelativeSSA(
			ulong size,
			SSARegisterStack registerStack,
			LowLevelILExpressionIndex entry,
			LowLevelILSSARegister top,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_REL_SSA,
				location,
				size,
				0,
				(ulong)registerStack.RegisterStack.Index,
				registerStack.Version,
				(ulong)entry,
				(ulong)this.EmitRegisterSSA(0, top, location));
		}

		public LowLevelILExpressionIndex EmitRegisterStackAbsoluteSSA(
			ulong size,
			SSARegisterStack registerStack,
			RegisterIndex register,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_ABS_SSA,
				location,
				size,
				0,
				(ulong)registerStack.RegisterStack.Index,
				registerStack.Version,
				(ulong)register);
		}

		public LowLevelILExpressionIndex EmitRegisterStackFreeTopRelativeSSA(
			RegisterStackIndex registerStack,
			ulong destinationVersion,
			ulong sourceVersion,
			LowLevelILExpressionIndex entry,
			LowLevelILSSARegister top,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_FREE_REL_SSA,
				location,
				0,
				0,
				(ulong)this.EmitRegisterStackDestinationSSA(
					0,
					registerStack,
					destinationVersion,
					sourceVersion,
					location),
				(ulong)entry,
				(ulong)this.EmitRegisterSSA(0, top, location));
		}

		public LowLevelILExpressionIndex EmitRegisterStackFreeAbsoluteSSA(
			RegisterStackIndex registerStack,
			ulong destinationVersion,
			ulong sourceVersion,
			RegisterIndex register,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_FREE_ABS_SSA,
				location,
				0,
				0,
				(ulong)this.EmitRegisterStackDestinationSSA(
					0,
					registerStack,
					destinationVersion,
					sourceVersion,
					location),
				(ulong)register);
		}

		public LowLevelILExpressionIndex EmitFlagSSA(
			LowLevelILSSAFlag flag,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_FLAG_SSA,
				location,
				0,
				0,
				(ulong)flag.Flag.Index,
				flag.Version);
		}

		public LowLevelILExpressionIndex EmitFlagBitSSA(
			ulong size,
			LowLevelILSSAFlag flag,
			ulong bitIndex,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_FLAG_BIT_SSA,
				location,
				size,
				0,
				(ulong)flag.Flag.Index,
				flag.Version,
				bitIndex);
		}

		public LowLevelILExpressionIndex EmitRegisterPhi(
			LowLevelILSSARegister destination,
			LowLevelILSSARegister[] sources,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_PHI,
				location,
				0,
				0,
				(ulong)destination.Register.Index,
				destination.Version,
				(ulong)sources.Length * 2,
				(ulong)this.AddSSARegisterList(sources));
		}

		public LowLevelILExpressionIndex EmitRegisterStackPhi(
			SSARegisterStack destination,
			SSARegisterStack[] sources,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_REG_STACK_PHI,
				location,
				0,
				0,
				(ulong)destination.RegisterStack.Index,
				destination.Version,
				(ulong)sources.Length * 2,
				(ulong)this.AddSSARegisterStackList(sources));
		}

		public LowLevelILExpressionIndex EmitFlagPhi(
			LowLevelILSSAFlag destination,
			LowLevelILSSAFlag[] sources,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_FLAG_PHI,
				location,
				0,
				0,
				(ulong)destination.Flag.Index,
				destination.Version,
				(ulong)sources.Length * 2,
				(ulong)this.AddSSAFlagList(sources));
		}

		public LowLevelILExpressionIndex EmitMemoryPhi(
			ulong destinationMemoryVersion,
			ulong[] sourceMemoryVersions,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_MEM_PHI,
				location,
				0,
				0,
				destinationMemoryVersion,
				(ulong)sourceMemoryVersions.Length,
				(ulong)this.AddIndexList(sourceMemoryVersions));
		}
	}
}
