namespace BinaryNinja
{
	public sealed class HLILLowPart : AbstractHighLevelILUnaryInstruction
	{
		internal HLILLowPart(
			HighLevelILFunction ilFunction ,
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}

	}
}
