namespace BinaryNinja
{
	public sealed class HLILAddCarry : AbstractHighLevelILCarryInstruction
	{
		internal HLILAddCarry(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		
		
	}
}
