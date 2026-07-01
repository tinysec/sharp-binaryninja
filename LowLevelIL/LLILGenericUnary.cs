namespace BinaryNinja
{
	/// <summary>
	/// Shared wrapper for unary Low Level IL operations that carry a single
	/// <c>src</c> expression operand and had no dedicated class (ABS, BSWAP, CLS,
	/// CLZ, CTZ, POPCNT, RBIT). The specific operation is available via
	/// <see cref="LowLevelILInstruction.Operation"/>.
	/// </summary>
	public sealed class LLILGenericUnary : LowLevelILInstruction
	{
		internal LLILGenericUnary(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{

		}

		public LowLevelILInstruction Source
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}
	}
}
