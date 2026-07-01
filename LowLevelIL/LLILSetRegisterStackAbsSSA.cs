namespace BinaryNinja
{
	public sealed class LLILSetRegisterStackAbsSSA : LowLevelILInstruction
	{
		internal LLILSetRegisterStackAbsSSA(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public LowLevelILInstruction Stack
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)0);
			}
		}
		
		public ILRegister Destination
		{
			get
			{
				return this.GetOperandAsRegister((OperandIndex)1);
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
