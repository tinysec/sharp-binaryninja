namespace BinaryNinja
{
	public sealed class HLILFloatEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
