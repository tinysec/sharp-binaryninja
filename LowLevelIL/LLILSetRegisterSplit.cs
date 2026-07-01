namespace BinaryNinja
{
	public sealed class LLILSetRegisterSplit : LowLevelILInstruction
	{
		internal LLILSetRegisterSplit(
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
				return this.GetOperandAsRegister((OperandIndex)0 );
			}
		}
		
		public ILRegister Low
		{
			get
			{
				return this.GetOperandAsRegister( (OperandIndex)1 );
			}
		}
		
		public LowLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression( (OperandIndex)2 );
			}
		}
	}
}
