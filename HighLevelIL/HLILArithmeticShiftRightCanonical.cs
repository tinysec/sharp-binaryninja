namespace BinaryNinja
{
	public class HLILArithmeticShiftRight : AbstractHighLevelILBinaryInstruction
	{
		internal HLILArithmeticShiftRight(
			HighLevelILFunction ilFunction,
			HighLevelILExpressionIndex expressionIndex,
			BNHighLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}
	}
}
