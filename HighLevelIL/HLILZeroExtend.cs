namespace BinaryNinja
{
	public sealed class HLILZeroExtend : AbstractHighLevelILUnaryInstruction
	{
		internal HLILZeroExtend(
			HighLevelILFunction ilFunction ,
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}

	}
}
