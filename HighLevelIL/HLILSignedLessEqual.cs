namespace BinaryNinja
{
	public sealed class HLILSignedLessEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILSignedLessEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
