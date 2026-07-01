namespace BinaryNinja
{
	public sealed class LLILFlagBit : LowLevelILInstruction
	{
		internal LLILFlagBit(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{
			
		}
		
		public ILFlag Source
		{
			get
			{
				return this.GetOperandAsFlag(0);
			}
		}
		
		public ulong Bit
		{
			get
			{
				return this.RawOperands[1];
			}
		}
	}
}
