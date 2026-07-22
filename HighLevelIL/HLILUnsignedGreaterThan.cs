namespace BinaryNinja
{
	public sealed class HLILUnsignedGreaterThan : AbstractHighLevelILBinaryInstruction
	{
		internal HLILUnsignedGreaterThan(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
