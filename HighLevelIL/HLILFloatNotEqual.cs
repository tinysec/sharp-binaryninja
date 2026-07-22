namespace BinaryNinja
{
	public sealed class HLILFloatNotEqual : AbstractHighLevelILBinaryInstruction
	{
		internal HLILFloatNotEqual(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
