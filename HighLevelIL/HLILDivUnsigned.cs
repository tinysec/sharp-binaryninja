namespace BinaryNinja
{
	public sealed class HLILDivUnsigned : AbstractHighLevelILBinaryInstruction
	{
		internal HLILDivUnsigned(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
