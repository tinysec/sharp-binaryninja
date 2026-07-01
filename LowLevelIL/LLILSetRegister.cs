namespace BinaryNinja
{
	public sealed class LLILSetRegister : LowLevelILInstruction
	{
		internal LLILSetRegister(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}

		public ILRegister Destionation
		{
			get
			{
				return this.GetOperandAsRegister(0);
			}
		}
		
		public LowLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}
	}
}
