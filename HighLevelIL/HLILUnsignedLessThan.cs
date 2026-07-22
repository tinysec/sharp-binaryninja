namespace BinaryNinja
{
	public sealed class HLILUnsignedLessThan : AbstractHighLevelILBinaryInstruction
	{
		internal HLILUnsignedLessThan(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
