namespace BinaryNinja
{
	public abstract class AbstractHighLevelILUnaryInstruction : HighLevelILInstruction
	{
		internal AbstractHighLevelILUnaryInstruction(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public HighLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}

		public HighLevelILInstruction Left
		{
			get
			{
				return this.Source;
			}
		}
	}
}
