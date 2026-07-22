namespace BinaryNinja
{
	public sealed class HLILUnsignedGreaterEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILUnsignedGreaterEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
