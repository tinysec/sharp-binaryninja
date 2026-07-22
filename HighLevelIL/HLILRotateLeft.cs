namespace BinaryNinja
{
	public sealed class HLILRotateLeft : AbstractHighLevelILBinaryInstruction
	{
		internal HLILRotateLeft(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
