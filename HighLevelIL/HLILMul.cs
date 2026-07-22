namespace BinaryNinja
{
	public sealed class HLILMul : AbstractHighLevelILBinaryInstruction
	{
		internal HLILMul(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		
	}
}
