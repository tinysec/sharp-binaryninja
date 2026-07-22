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

		public ILFlag Destination
		{
			get
			{
				return this.GetOperandAsFlag(0);
			}
		}

		[System.Obsolete("Use Destination instead.")]
		public ILFlag Destionation
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
