namespace BinaryNinja
{
	public sealed class HLILDivSignedDoublePrecision : AbstractHighLevelILBinaryInstruction
	{
		internal HLILDivSignedDoublePrecision(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
