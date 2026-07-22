namespace BinaryNinja
{
	public sealed class HLILRoundToInt : AbstractHighLevelILUnaryInstruction
	{
		internal HLILRoundToInt(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
