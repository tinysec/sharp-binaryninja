namespace BinaryNinja
{
	public sealed class LLILRegister : LowLevelILInstruction
	{
		internal LLILRegister(
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
				return this.GetOperandAsRegister(0);
			}
		}
	}
}
