namespace BinaryNinja
{
	public sealed class HLILOr : AbstractHighLevelILBinaryInstruction
	{
		internal HLILOr(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
