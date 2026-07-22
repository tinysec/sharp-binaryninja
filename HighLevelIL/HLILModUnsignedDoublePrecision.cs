namespace BinaryNinja
{
	public sealed class HLILModUnsignedDoublePrecision : AbstractHighLevelILBinaryInstruction
	{
		internal HLILModUnsignedDoublePrecision(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
