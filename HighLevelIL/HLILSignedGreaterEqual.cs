namespace BinaryNinja
{
	public sealed class HLILSignedGreaterEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILSignedGreaterEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
