namespace BinaryNinja
{
	public sealed class HLILConstData : AbstractHighLevelILConstDataInstruction
	{
		internal HLILConstData(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
	}
}
