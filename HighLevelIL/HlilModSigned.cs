namespace BinaryNinja
{
	[System.Obsolete("Use HLILModSigned instead.")]
	public sealed class HlilModSigned : HLILModSigned
	{
		internal HlilModSigned(
			HighLevelILFunction ilFunction, 
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex, native)
		{
			
		}
	}
}
