namespace BinaryNinja
{
	public sealed class HLILSignedLessThan : AbstractHighLevelILBinaryInstruction
	{
		internal HLILSignedLessThan(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
