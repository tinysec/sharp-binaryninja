namespace BinaryNinja
{
	public sealed class LLILAssert : LowLevelILInstruction
	{
		internal LLILAssert(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public ILRegister Source
		{
			get
			{
				return this.GetOperandAsRegister((OperandIndex)0);
			}
		}
		
		public PossibleValueSet Constraint
		{
			get
			{
				return this.GetOperandAsPossibleValueSet((OperandIndex)1);
			}
		}
	}
}
