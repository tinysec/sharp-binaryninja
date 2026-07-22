namespace BinaryNinja
{
	public sealed class HLILFloatDiv : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatDiv(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
