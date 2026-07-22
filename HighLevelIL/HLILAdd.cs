namespace BinaryNinja
{
	public sealed class HLILAdd : AbstractHighLevelILBinaryInstruction
	{
		internal HLILAdd(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
