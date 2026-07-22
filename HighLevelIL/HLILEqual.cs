namespace BinaryNinja
{
	public sealed class HLILEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
