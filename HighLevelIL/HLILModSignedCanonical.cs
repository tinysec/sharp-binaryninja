namespace BinaryNinja
{
	public class HLILModSigned : AbstractHighLevelILBinaryInstruction
	{
		internal HLILModSigned(
			HighLevelILFunction ilFunction,
			HighLevelILExpressionIndex expressionIndex,
			BNHighLevelILInstruction native)
			: base(ilFunction, expressionIndex, native)
		{
		}
	}
}
