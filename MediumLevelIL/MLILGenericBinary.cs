namespace BinaryNinja
{
	/// <summary>
	/// Shared wrapper for binary Medium Level IL operations that carry
	/// <c>left</c>/<c>right</c> expression operands and had no dedicated class
	/// (MAXS, MAXU, MINS, MINU). The specific operation is available via
	/// <see cref="MediumLevelILInstruction.Operation"/>.
	/// </summary>
	public sealed class MLILGenericBinary : MediumLevelILInstruction
	{
		internal MLILGenericBinary(
			MediumLevelILFunction ilFunction ,
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}

		public MediumLevelILInstruction Left
		{
			get
			{
				return this.GetOperandAsExpression(0);
			}
		}

		public MediumLevelILInstruction Right
		{
			get
			{
				return this.GetOperandAsExpression((OperandIndex)1);
			}
		}
	}
}
