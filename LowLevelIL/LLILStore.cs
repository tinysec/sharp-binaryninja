namespace BinaryNinja
{
	public sealed class LLILStore : LowLevelILInstruction
	{
		internal LLILStore(
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

		[System.Obsolete("Use Destination instead.")]
		public LowLevelILInstruction Destionation
		{
			get
			{
				return this.Destination;
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
