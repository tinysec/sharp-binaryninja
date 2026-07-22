namespace BinaryNinja
{
	public sealed class HLILFloatAbs : AbstractHighLevelILUnaryInstruction
	{
		internal HLILFloatAbs(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
