namespace BinaryNinja
{
	public sealed class HLILLogicalShiftRight : AbstractHighLevelILBinaryInstruction
	{
		internal HLILLogicalShiftRight(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
