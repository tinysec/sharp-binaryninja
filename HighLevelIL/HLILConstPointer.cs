namespace BinaryNinja
{
	public sealed class HLILConstPointer : AbstractHighLevelILConstInstruction
	{
		internal HLILConstPointer(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
