namespace BinaryNinja
{
	public sealed class HLILMulUnsignedDoublePrecision : AbstractHighLevelILBinaryInstruction
	{
		internal HLILMulUnsignedDoublePrecision(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
