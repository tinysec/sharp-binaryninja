namespace BinaryNinja
{
	public sealed class LLILRegisterSSAPartial : LowLevelILInstruction
	{
		internal LLILRegisterSSAPartial(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public LowLevelILSSARegister FullRegister
		{
			get
			{
				return this.GetOperandAsSSARegister((OperandIndex)0,(OperandIndex)1);
			}
		}
		
		public ILRegister Source
		{
			get
			{
				return this.GetOperandAsRegister((OperandIndex)2);
			}
		}
	}
}
