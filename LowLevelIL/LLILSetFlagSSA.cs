namespace BinaryNinja
{
	public sealed class LLILSetFlagSSA : LowLevelILInstruction
	{
		internal LLILSetFlagSSA(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public LowLevelILSSAFlag Destination
		{
			get
			{
				return this.GetOperandAsSSAFlag((OperandIndex)0 , (OperandIndex)1);
			}
		}
		
		public LowLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)2);
			}
		}
	}
}
