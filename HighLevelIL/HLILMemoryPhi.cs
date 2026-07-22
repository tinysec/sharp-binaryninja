namespace BinaryNinja
{
	public sealed class HLILMemoryPhi : HighLevelILInstruction
	{
		internal HLILMemoryPhi(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		public ulong DestinationMemory
		{
			get
			{
				return this.RawOperands[0];
			}
		}

		[System.Obsolete("Use DestinationMemory instead.")]
		public ulong Destination
		{
			get
			{
				return this.DestinationMemory;
			}
		}
		
		public ulong[] SourceMemory
		{
			get
			{
				return this.GetOperandAsIndexList((OperandIndex)1);
			}
		}

		[System.Obsolete("Use SourceMemory instead.")]
		public ulong[] Source
		{
			get
			{
				return this.SourceMemory;
			}
		}
	}
}
