namespace BinaryNinja
{
	[System.Obsolete("Use HLILLogicalShiftLeft instead.")]
	public sealed class HLILLogicalShiftLef : HLILLogicalShiftLeft
	{
		internal HLILLogicalShiftLef(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
	}
}
