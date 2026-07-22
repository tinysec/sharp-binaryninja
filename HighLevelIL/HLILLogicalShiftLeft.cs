namespace BinaryNinja
{
	public class HLILLogicalShiftLeft : AbstractHighLevelILBinaryInstruction
	{
		internal HLILLogicalShiftLeft(
			HighLevelILFunction ilFunction,
			HighLevelILExpressionIndex expressionIndex,
			BNHighLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}
	}
}
