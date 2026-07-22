namespace BinaryNinja
{
	public sealed class HLILTestBit : AbstractHighLevelILBinaryInstruction
	{
		internal HLILTestBit(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
