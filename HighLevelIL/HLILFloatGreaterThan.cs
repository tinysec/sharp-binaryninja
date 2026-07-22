namespace BinaryNinja
{
	public sealed class HLILFloatGreaterThan : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatGreaterThan(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
