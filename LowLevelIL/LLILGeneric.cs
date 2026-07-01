namespace BinaryNinja
{
	/// <summary>
	/// Fallback wrapper for Low Level IL operations that have no dedicated class
	/// yet (or operations added in a newer core than this binding). Generic
	/// navigation (operation, address, tokens, cross-level, SSA) still works;
	/// typed operand accessors are not exposed. Use
	/// <see cref="LowLevelILInstruction.RawOperands"/> if needed.
	/// </summary>
	public sealed class LLILGeneric : LowLevelILInstruction
	{
		internal LLILGeneric(
			LowLevelILFunction function ,
			LowLevelILExpressionIndex expressionIndex ,
			BNLowLevelILInstruction native
		) : base(function , expressionIndex , native)
		{

		}
	}
}
