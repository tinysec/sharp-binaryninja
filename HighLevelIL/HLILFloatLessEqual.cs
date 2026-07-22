namespace BinaryNinja
{
	public sealed class HLILFloatLessEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatLessEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
