namespace BinaryNinja
{
	public sealed class HLILFloatNeg : AbstractHighLevelILUnaryInstruction
	{
		internal HLILFloatNeg(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
