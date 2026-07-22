namespace BinaryNinja
{
	public sealed class HLILCeil : AbstractHighLevelILUnaryInstruction
	{
		internal HLILCeil(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
