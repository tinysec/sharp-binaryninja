namespace BinaryNinja
{
	/// <summary>
	/// Shared wrapper for unary High Level IL operations that carry a single
	/// <c>src</c> expression operand and had no dedicated class (ABS, BSWAP, CLS,
	/// CLZ, CTZ, POPCNT, RBIT). Exposes <see cref="AbstractHighLevelILUnaryInstruction.Source"/>.
	/// The specific operation is available via <see cref="HighLevelILInstruction.Operation"/>.
	/// </summary>
	public sealed class HLILGenericUnary : AbstractHighLevelILUnaryInstruction
	{
		internal HLILGenericUnary(
			HighLevelILFunction ilFunction ,
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}
	}
}
