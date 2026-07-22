namespace BinaryNinja
{
	public sealed class LLILFlagCond : LowLevelILInstruction
	{
		internal LLILFlagCond(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native 
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public LowLevelILFlagCondition Condition
		{
			get
			{
				return this.GetOperandAsFlagCondition(0);
			}
		}
		
		public SemanticFlagClass SemanticFlagClass
		{
			get
			{
				return this.GetOperandAsSemanticFlagClass((OperandIndex)1);
			}
		}

		public SemanticFlagClass SemanticClass
		{
			get
			{
				return this.SemanticFlagClass;
			}
		}
	}
}
