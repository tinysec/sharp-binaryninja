namespace BinaryNinja
{
	public sealed class HLILRotateLeftCarry : AbstractHighLevelILCarryInstruction
	{
		internal HLILRotateLeftCarry(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
		
		
	}
}
