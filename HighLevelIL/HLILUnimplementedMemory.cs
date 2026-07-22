namespace BinaryNinja
{
	public sealed class HLILUnimplementedMemory : AbstractHighLevelILUnaryInstruction
	{
		internal HLILUnimplementedMemory(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
