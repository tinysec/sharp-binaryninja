namespace BinaryNinja
{
	public sealed class HLILTrap : HighLevelILInstruction
	{
		internal HLILTrap(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public long Vector
		{
			get
			{
				return (long)this.RawOperands[0];
			}
		}

		public ulong RawVector
		{
			get
			{
				return this.RawOperands[0];
			}
		}
	}
}
