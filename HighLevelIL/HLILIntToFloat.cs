namespace BinaryNinja
{
	public sealed class HLILIntToFloat : AbstractHighLevelILUnaryInstruction
	{
		internal HLILIntToFloat(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
		
	}
}
