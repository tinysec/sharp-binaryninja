namespace BinaryNinja
{
	/// <summary>
	/// Shared wrapper for unary Medium Level IL operations that carry a single
	/// <c>src</c> expression operand and had no dedicated class (ABS, BSWAP, CLS,
	/// CLZ, CTZ, POPCNT, RBIT). The specific operation is available via
	/// <see cref="MediumLevelILInstruction.Operation"/>.
	/// </summary>
	public sealed class MLILGenericUnary : AbstractMediumLevelILUnaryInstruction
	{
		internal MLILGenericUnary(
			MediumLevelILFunction ilFunction ,
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}

	}
}
