namespace BinaryNinja
{
	public sealed class HLILFloatSquareRoot : AbstractHighLevelILUnaryInstruction
	{
		internal HLILFloatSquareRoot(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
