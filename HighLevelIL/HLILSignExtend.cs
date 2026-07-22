namespace BinaryNinja
{
	public sealed class HLILSignExtend : AbstractHighLevelILUnaryInstruction
	{
		internal HLILSignExtend(
			HighLevelILFunction ilFunction ,
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}

	}
}
