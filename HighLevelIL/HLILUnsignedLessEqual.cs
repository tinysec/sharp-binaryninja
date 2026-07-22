namespace BinaryNinja
{
	public sealed class HLILUnsignedLessEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILUnsignedLessEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
