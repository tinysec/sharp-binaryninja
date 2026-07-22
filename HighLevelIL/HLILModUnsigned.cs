namespace BinaryNinja
{
	public sealed class HLILModUnsigned : AbstractHighLevelILBinaryInstruction
	{
		internal HLILModUnsigned(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
