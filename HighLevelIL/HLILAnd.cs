namespace BinaryNinja
{
	public sealed class HLILAnd : AbstractHighLevelILBinaryInstruction
	{
		internal HLILAnd(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
