namespace BinaryNinja
{
	public sealed class LLILRegisterStackFreeAbsSSA : LowLevelILInstruction
	{
		internal LLILRegisterStackFreeAbsSSA(
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
	}
}
