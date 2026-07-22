namespace BinaryNinja
{
	public sealed class HLILSub : AbstractHighLevelILBinaryInstruction
	{
		internal HLILSub(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
