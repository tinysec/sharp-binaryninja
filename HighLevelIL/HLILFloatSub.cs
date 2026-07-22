namespace BinaryNinja
{
	public sealed class HLILFloatSub : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatSub(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
