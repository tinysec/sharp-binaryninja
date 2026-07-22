namespace BinaryNinja
{
	public sealed class LLILFlagGroup : LowLevelILInstruction
	{
		internal LLILFlagGroup(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public SemanticFlagGroup SemanticFlagGroup
		{
			get
			{
				return this.GetOperandAsSemanticFlagGroup(0);
			}
		}

		public SemanticFlagGroup SemanticGroup
		{
			get
			{
				return this.SemanticFlagGroup;
			}
		}
	}
}
