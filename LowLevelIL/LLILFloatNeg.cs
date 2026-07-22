namespace BinaryNinja
{
	public sealed class LLILFloatNeg : LowLevelILInstruction
	{
		internal LLILFloatNeg(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}

		public LowLevelILInstruction Destination
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}

		public LowLevelILInstruction Source
		{
			get
			{
				return this.Destination;
			}
		}
	}
}
