namespace BinaryNinja
{
	public sealed class HLILSignedGreaterThan : AbstractHighLevelILBinaryInstruction
	{
		internal HLILSignedGreaterThan(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
