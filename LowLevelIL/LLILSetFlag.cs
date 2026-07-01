namespace BinaryNinja
{
	public sealed class LLILSetFlag : LowLevelILInstruction
	{
		internal LLILSetFlag(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}

		public ILFlag Destionation
		{
			get
			{
				return this.GetOperandAsFlag(0);
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
