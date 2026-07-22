namespace BinaryNinja
{
	public sealed class HLILImport : AbstractHighLevelILConstInstruction
	{
		internal HLILImport(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
