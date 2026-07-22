namespace BinaryNinja
{
	public sealed class HLILMulSignedDoublePrecision : AbstractHighLevelILBinaryInstruction
	{
		internal HLILMulSignedDoublePrecision(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
