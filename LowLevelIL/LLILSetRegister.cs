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

		public ILRegister Destination
		{
			get
			{
				return this.GetOperandAsRegister(0);
			}
		}

		[System.Obsolete("Use Destination instead.")]
		public ILRegister Destionation
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
