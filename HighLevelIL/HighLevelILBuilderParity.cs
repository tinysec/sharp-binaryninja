namespace BinaryNinja
{
	public sealed partial class HighLevelILFunction
	{
		public HighLevelILExpressionIndex EmitSwitch(
			HighLevelILExpressionIndex condition,
			HighLevelILExpressionIndex defaultExpression,
			HighLevelILExpressionIndex[] cases,
			SourceLocation? location = null)
		{
			return this.AddExpression(
				HighLevelILOperation.HLIL_SWITCH,
				location,
				0,
				(ulong)condition,
				(ulong)defaultExpression,
				(ulong)cases.Length,
				(ulong)this.AddOperandList(cases));
		}

		public HighLevelILExpressionIndex EmitReturn(
			HighLevelILExpressionIndex[] sources,
			SourceLocation? location = null)
		{
			return this.EmitRet(sources, location);
		}

		public HighLevelILExpressionIndex EmitNoReturn(SourceLocation? location = null)
		{
			return this.EmitNoRet(location);
		}

		public HighLevelILExpressionIndex EmitForceVer(
			ulong size,
			Variable destination,
			Variable source,
			SourceLocation? location = null)
		{
			return this.EmitForceVersion(size, destination, source, location);
		}

		public HighLevelILExpressionIndex EmitForceVerSSA(
			ulong size,
			HighLevelILSSAVariable destination,
			HighLevelILSSAVariable source,
			SourceLocation? location = null)
		{
			return this.EmitForceVersionSSA(size, destination, source, location);
		}
	}
}
