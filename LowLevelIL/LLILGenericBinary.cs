namespace BinaryNinja
{
	/// <summary>
	/// Shared wrapper for binary Low Level IL operations that carry
	/// <c>left</c>/<c>right</c> expression operands and had no dedicated class
	/// (MAXS, MAXU, MINS, MINU). The specific operation is available via
	/// <see cref="LowLevelILInstruction.Operation"/>.
	/// </summary>
	public sealed class LLILGenericBinary : LowLevelILInstruction
	{
		internal LLILGenericBinary(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{

		}

		public LowLevelILInstruction Left
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}

		public LowLevelILInstruction Right
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}
	}
}
