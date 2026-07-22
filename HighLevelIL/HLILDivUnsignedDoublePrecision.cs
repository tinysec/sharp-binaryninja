namespace BinaryNinja
{
	public sealed class HLILDivUnsignedDoublePrecision : AbstractHighLevelILBinaryInstruction
	{
		internal HLILDivUnsignedDoublePrecision(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
