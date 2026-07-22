namespace BinaryNinja
{
	public sealed class HLILSubBorrow : AbstractHighLevelILCarryInstruction
	{
		internal HLILSubBorrow(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		
		
	}
}
