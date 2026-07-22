namespace BinaryNinja
{
	public sealed class HLILModSignedDoublePrecision : AbstractHighLevelILBinaryInstruction
	{
		internal HLILModSignedDoublePrecision(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
