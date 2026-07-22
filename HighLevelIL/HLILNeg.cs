namespace BinaryNinja
{
	public sealed class HLILNeg : AbstractHighLevelILUnaryInstruction
	{
		internal HLILNeg(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
