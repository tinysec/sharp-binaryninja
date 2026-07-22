namespace BinaryNinja
{
	public sealed class LLILMemoryPhi : LowLevelILInstruction
	{
		internal LLILMemoryPhi(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
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
		public ulong DestionationMemory
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
	}
}
