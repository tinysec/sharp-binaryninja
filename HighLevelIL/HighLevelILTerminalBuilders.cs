namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		public HighLevelILExpressionIndex EmitBreakpoint(SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_BP,
				location,
				0);
		}

		public HighLevelILExpressionIndex EmitTrap(
			long vector,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_TRAP,
				location,
				0,
				(ulong)vector);
		}

		public HighLevelILExpressionIndex EmitUndefined(SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_UNDEF,
				location,
				0);
		}

		public HighLevelILExpressionIndex EmitUnimplemented(SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_UNIMPL,
				location,
				0);
		}

		public HighLevelILExpressionIndex EmitUnimplementedMemoryRef(
			ulong size,
			HighLevelILExpressionIndex target,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_UNIMPL_MEM,
				location,
				size,
				(ulong)target);
		}
	}
}
