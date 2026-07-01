namespace BinaryNinja
{
	public sealed class LLILRegisterSplit : LowLevelILInstruction
	{
		internal LLILRegisterSplit(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}

		public ILRegister High
		{
			get
			{
				return this.GetOperandAsRegister(0);
			}
		}
		
		public ILRegister Low
		{
			get
			{
				return this.GetOperandAsRegister((OperandIndex)1);
			}
		}
	}
}
