namespace BinaryNinja
{
	public sealed class HLILFloatToInt : AbstractHighLevelILUnaryInstruction
	{
		internal HLILFloatToInt(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
