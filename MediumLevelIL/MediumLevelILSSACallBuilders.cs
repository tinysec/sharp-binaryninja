namespace BinaryNinja
{
	public sealed partial class MediumLevelILFunction
	{
		public MediumLevelILExpressionIndex EmitCallOutputSSA(
			MediumLevelILSSAVariable[] outputs,
			ulong destinationMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_CALL_OUTPUT_SSA,
				location,
				0,
				destinationMemoryVersion,
				(ulong)outputs.Length * 2,
				(ulong)this.AddSSAVariableList(outputs));
		}

		public MediumLevelILExpressionIndex EmitCallParamSSA(
			MediumLevelILExpressionIndex[] parameters,
			ulong sourceMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_CALL_PARAM_SSA,
				location,
				0,
				sourceMemoryVersion,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public MediumLevelILExpressionIndex EmitCallSSA(
			MediumLevelILSSAVariable[] outputs,
			MediumLevelILExpressionIndex destination,
			MediumLevelILExpressionIndex[] parameters,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_CALL_SSA,
				location,
				0,
				(ulong)this.EmitCallOutputSSA(outputs, newMemoryVersion, location),
				(ulong)destination,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters),
				previousMemoryVersion);
		}

		public MediumLevelILExpressionIndex EmitCallUntypedSSA(
			MediumLevelILSSAVariable[] outputs,
			MediumLevelILExpressionIndex destination,
			MediumLevelILExpressionIndex[] parameters,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			MediumLevelILExpressionIndex stack,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_CALL_UNTYPED_SSA,
				location,
				0,
				(ulong)this.EmitCallOutputSSA(outputs, newMemoryVersion, location),
				(ulong)destination,
				(ulong)this.EmitCallParamSSA(parameters, previousMemoryVersion, location),
				(ulong)stack);
		}

		public MediumLevelILExpressionIndex EmitSysCallSSA(
			MediumLevelILSSAVariable[] outputs,
			MediumLevelILExpressionIndex[] parameters,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_SYSCALL_SSA,
				location,
				0,
				(ulong)this.EmitCallOutputSSA(outputs, newMemoryVersion, location),
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters),
				previousMemoryVersion);
		}

		public MediumLevelILExpressionIndex EmitSysCallUntypedSSA(
			MediumLevelILSSAVariable[] outputs,
			MediumLevelILExpressionIndex[] parameters,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			MediumLevelILExpressionIndex stack,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_SYSCALL_UNTYPED_SSA,
				location,
				0,
				(ulong)this.EmitCallOutputSSA(outputs, newMemoryVersion, location),
				(ulong)this.EmitCallParamSSA(parameters, previousMemoryVersion, location),
				(ulong)stack);
		}

		public MediumLevelILExpressionIndex EmitTailCallSSA(
			MediumLevelILSSAVariable[] outputs,
			MediumLevelILExpressionIndex destination,
			MediumLevelILExpressionIndex[] parameters,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_TAILCALL_SSA,
				location,
				0,
				(ulong)this.EmitCallOutputSSA(outputs, newMemoryVersion, location),
				(ulong)destination,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters),
				previousMemoryVersion);
		}

		public MediumLevelILExpressionIndex EmitTailCallUntypedSSA(
			MediumLevelILSSAVariable[] outputs,
			MediumLevelILExpressionIndex destination,
			MediumLevelILExpressionIndex[] parameters,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			MediumLevelILExpressionIndex stack,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_TAILCALL_UNTYPED_SSA,
				location,
				0,
				(ulong)this.EmitCallOutputSSA(outputs, newMemoryVersion, location),
				(ulong)destination,
				(ulong)this.EmitCallParamSSA(parameters, previousMemoryVersion, location),
				(ulong)stack);
		}

		public MediumLevelILExpressionIndex EmitIntrinsicSSA(
			MediumLevelILSSAVariable[] outputs,
			IntrinsicIndex intrinsic,
			MediumLevelILExpressionIndex[] parameters,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_INTRINSIC_SSA,
				location,
				0,
				(ulong)outputs.Length * 2,
				(ulong)this.AddSSAVariableList(outputs),
				(ulong)intrinsic,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters));
		}

		public MediumLevelILExpressionIndex EmitMemoryIntrinsicOutputSSA(
			MediumLevelILSSAVariable[] outputs,
			ulong destinationMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_MEMORY_INTRINSIC_OUTPUT_SSA,
				location,
				0,
				destinationMemoryVersion,
				(ulong)outputs.Length * 2,
				(ulong)this.AddSSAVariableList(outputs));
		}

		public MediumLevelILExpressionIndex EmitMemoryIntrinsicSSA(
			MediumLevelILSSAVariable[] outputs,
			IntrinsicIndex intrinsic,
			MediumLevelILExpressionIndex[] parameters,
			ulong newMemoryVersion,
			ulong previousMemoryVersion,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				MediumLevelILOperation.MLIL_MEMORY_INTRINSIC_SSA,
				location,
				0,
				(ulong)this.EmitMemoryIntrinsicOutputSSA(outputs, newMemoryVersion, location),
				(ulong)intrinsic,
				(ulong)parameters.Length,
				(ulong)this.AddOperandList(parameters),
				previousMemoryVersion);
		}
	}
}
