namespace BinaryNinja
{
	public sealed class HLILRotateRight : AbstractHighLevelILBinaryInstruction
	{
		internal HLILRotateRight(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
