namespace BinaryNinja
{
	public sealed class HLILFloatAdd : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatAdd(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
