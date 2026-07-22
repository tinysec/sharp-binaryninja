namespace BinaryNinja
{
	public sealed class HLILFloatConvert : AbstractHighLevelILUnaryInstruction
	{
		internal HLILFloatConvert(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
