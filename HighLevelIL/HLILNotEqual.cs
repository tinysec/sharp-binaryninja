namespace BinaryNinja
{
	public sealed class HLILNotEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILNotEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
