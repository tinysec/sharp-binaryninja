namespace BinaryNinja
{
	public sealed class HLILFloatCompareUnordered : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatCompareUnordered(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
