namespace BinaryNinja
{
	public sealed class HLILFloatGreaterEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatGreaterEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
