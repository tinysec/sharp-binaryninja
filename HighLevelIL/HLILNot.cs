namespace BinaryNinja
{
	public sealed class HLILNot : AbstractHighLevelILUnaryInstruction
	{
		internal HLILNot(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
