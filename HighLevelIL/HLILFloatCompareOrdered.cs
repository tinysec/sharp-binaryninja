namespace BinaryNinja
{
	public sealed class HLILFloatCompareOrdered : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatCompareOrdered(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
