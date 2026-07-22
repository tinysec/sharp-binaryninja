namespace BinaryNinja
{
	public sealed class HLILXor : AbstractHighLevelILBinaryInstruction
	{
		internal HLILXor(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
