namespace BinaryNinja
{
	public sealed class LLILRegisterStackAbsSSA : LowLevelILInstruction
	{
		internal LLILRegisterStackAbsSSA(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native )
		{
			
		}
		
		public SSARegisterStack Stack
		{
			get
			{
				return this.GetOperandAsSSARegisterStack((OperandIndex)0,(OperandIndex)1);
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
