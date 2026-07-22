namespace BinaryNinja
{
	public sealed partial class LowLevelILFunction
	{
		public LowLevelILExpressionIndex EmitCallOutputSSA(
			LowLevelILSSARegister[] output,
			ulong destinationMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_CALL_OUTPUT_SSA,
				location,
				0,
				0,
				destinationMemoryVersion,
				(ulong)output.Length * 2,
				(ulong)this.AddSSARegisterList(output));
		}

		public LowLevelILExpressionIndex EmitCallStackSSA(
			LowLevelILSSARegister stack,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_CALL_STACK_SSA,
				location,
				0,
				0,
				(ulong)stack.Register.Index,
				stack.Version,
				sourceMemoryVersion);
		}

		public LowLevelILExpressionIndex EmitCallParameter(
			LowLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_CALL_PARAM,
				location,
				0,
				0,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public LowLevelILExpressionIndex EmitCallSSA(
			LowLevelILSSARegister[] output,
			LowLevelILExpressionIndex destination,
			LowLevelILExpressionIndex[] parameters,
			LowLevelILSSARegister stack,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_CALL_SSA,
				location,
				0,
				0,
				(ulong)this.EmitCallOutputSSA(
					output,
					destinationMemoryVersion,
					location),
				(ulong)destination,
				(ulong)this.EmitCallStackSSA(stack, sourceMemoryVersion, location),
				(ulong)this.EmitCallParameter(parameters, location));
		}

		public LowLevelILExpressionIndex EmitSystemCallSSA(
			LowLevelILSSARegister[] output,
			LowLevelILExpressionIndex[] parameters,
			LowLevelILSSARegister stack,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SYSCALL_SSA,
				location,
				0,
				0,
				(ulong)this.EmitCallOutputSSA(
					output,
					destinationMemoryVersion,
					location),
				(ulong)this.EmitCallStackSSA(stack, sourceMemoryVersion, location),
				(ulong)this.EmitCallParameter(parameters, location));
		}

		public LowLevelILExpressionIndex EmitSysCallSSA(
			LowLevelILSSARegister[] output,
			LowLevelILExpressionIndex[] parameters,
			LowLevelILSSARegister stack,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.EmitSystemCallSSA(
				output,
				parameters,
				stack,
				destinationMemoryVersion,
				sourceMemoryVersion,
				location);
		}

		public LowLevelILExpressionIndex EmitTailCallSSA(
			LowLevelILSSARegister[] output,
			LowLevelILExpressionIndex destination,
			LowLevelILExpressionIndex[] parameters,
			LowLevelILSSARegister stack,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_TAILCALL_SSA,
				location,
				0,
				0,
				(ulong)this.EmitCallOutputSSA(
					output,
					destinationMemoryVersion,
					location),
				(ulong)destination,
				(ulong)this.EmitCallStackSSA(stack, sourceMemoryVersion, location),
				(ulong)this.EmitCallParameter(parameters, location));
		}

		public LowLevelILExpressionIndex EmitSeparateParamListSSA(
			LowLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SEPARATE_PARAM_LIST_SSA,
				location,
				0,
				0,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public LowLevelILExpressionIndex EmitSharedParamSlotSSA(
			LowLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_SHARED_PARAM_SLOT_SSA,
				location,
				0,
				0,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public LowLevelILExpressionIndex EmitIntrinsicSSA(
			SSAFlagOrRegister[] output,
			IntrinsicIndex intrinsic,
			LowLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_INTRINSIC_SSA,
				location,
				0,
				0,
				(ulong)output.Length * 2,
				(ulong)this.AddSSAFlagOrRegisterList(output),
				(ulong)intrinsic,
				(ulong)this.EmitCallParameter(parameters, location));
		}

		public LowLevelILExpressionIndex EmitMemoryIntrinsicOutputSSA(
			SSAFlagOrRegister[] output,
			ulong destinationMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_MEMORY_INTRINSIC_OUTPUT_SSA,
				location,
				0,
				0,
				destinationMemoryVersion,
				(ulong)output.Length * 2,
				(ulong)this.AddSSAFlagOrRegisterList(output));
		}

		public LowLevelILExpressionIndex EmitMemoryIntrinsicSSA(
			SSAFlagOrRegister[] output,
			IntrinsicIndex intrinsic,
			LowLevelILExpressionIndex[] parameters,
			ulong destinationMemoryVersion,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				LowLevelILOperation.LLIL_MEMORY_INTRINSIC_SSA,
				location,
				0,
				0,
				(ulong)this.EmitMemoryIntrinsicOutputSSA(
					output,
					destinationMemoryVersion,
					location),
				(ulong)intrinsic,
				(ulong)this.EmitCallParameter(parameters, location),
				sourceMemoryVersion);
		}
	}
}
