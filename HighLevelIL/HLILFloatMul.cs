namespace BinaryNinja
{
	public sealed class HLILFloatMul : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatMul(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
