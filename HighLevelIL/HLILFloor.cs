namespace BinaryNinja
{
	public sealed class HLILFloor : AbstractHighLevelILUnaryInstruction
	{
		internal HLILFloor(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
