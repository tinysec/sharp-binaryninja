namespace BinaryNinja
{
	public sealed class HLILIntrinsicSSA : HighLevelILInstruction
	{
		internal HLILIntrinsicSSA(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public Intrinsic Intrinsic
		{
			get
			{
				return this.GetOperandAsIntrinsic(0);
			}
		}
		
		public HighLevelILInstruction[] Parameters
		{
			get
			{
				return this.GetOperandAsExpressionList((OperandIndex)1);
			}
		}
		
		public ulong DestinationMemory
		{
			get
			{
				return this.RawOperands[3];
			}
		}

		[System.Obsolete("Use DestinationMemory instead.")]
		public ulong DestionationMemory
		{
			get
			{
				return this.DestinationMemory;
			}
		}
		
		public ulong SourceMemory
		{
			get
			{
				return this.RawOperands[4];
			}
		}
	}
}
