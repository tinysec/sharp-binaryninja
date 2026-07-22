namespace BinaryNinja
{
	public sealed class HLILFloatLessThan : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatLessThan(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
