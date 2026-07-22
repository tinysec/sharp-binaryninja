namespace BinaryNinja
{
	public sealed class HLILDeref : AbstractHighLevelILUnaryInstruction
	{
		internal HLILDeref(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
