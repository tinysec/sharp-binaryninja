namespace BinaryNinja
{
	/// <summary>
	/// Shared wrapper for binary High Level IL operations that carry
	/// <c>left</c>/<c>right</c> expression operands and had no dedicated class
	/// (MAXS, MAXU, MINS, MINU). Exposes
	/// <see cref="AbstractHighLevelILBinaryInstruction.Left"/>/<see cref="AbstractHighLevelILBinaryInstruction.Right"/>.
	/// The specific operation is available via <see cref="HighLevelILInstruction.Operation"/>.
	/// </summary>
	public sealed class HLILGenericBinary : AbstractHighLevelILBinaryInstruction
	{
		internal HLILGenericBinary(
			HighLevelILFunction ilFunction ,
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}
	}
}
