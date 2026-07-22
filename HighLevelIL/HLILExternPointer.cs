namespace BinaryNinja
{
	public sealed class HLILExternPointer : AbstractHighLevelILConstInstruction
	{
		internal HLILExternPointer(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		
		public ulong Offset
		{
			get
			{
				return this.RawOperands[1];
			}
		}
	}
}
