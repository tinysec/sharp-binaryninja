namespace BinaryNinja
{
	public sealed class HLILDivSigned : AbstractHighLevelILBinaryInstruction
	{
		internal HLILDivSigned(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
