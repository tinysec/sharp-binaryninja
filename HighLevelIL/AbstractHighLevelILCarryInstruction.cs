namespace BinaryNinja
{
	public abstract class AbstractHighLevelILCarryInstruction : HighLevelILInstruction
	{
		internal AbstractHighLevelILCarryInstruction(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public HighLevelILInstruction Left
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}
		
		public HighLevelILInstruction Right
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}
		
		public HighLevelILInstruction Carry
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)2);
			}
		}
	}
}
