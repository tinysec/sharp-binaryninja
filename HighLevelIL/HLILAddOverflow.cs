namespace BinaryNinja
{
	public sealed class HLILAddOverflow : AbstractHighLevelILBinaryInstruction
	{
		internal HLILAddOverflow(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
