namespace BinaryNinja
{
	public sealed class HLILBoolToInt : AbstractHighLevelILUnaryInstruction
	{
		internal HLILBoolToInt(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
