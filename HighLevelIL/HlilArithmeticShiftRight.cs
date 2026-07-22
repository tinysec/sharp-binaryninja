namespace BinaryNinja
{
	[System.Obsolete("Use HLILArithmeticShiftRight instead.")]
	public sealed class HlilArithmeticShiftRight : HLILArithmeticShiftRight
	{
		internal HlilArithmeticShiftRight(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
	}
}
