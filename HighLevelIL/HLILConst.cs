namespace BinaryNinja
{
	public sealed class HLILConst : AbstractHighLevelILConstInstruction
	{
		internal HLILConst(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
