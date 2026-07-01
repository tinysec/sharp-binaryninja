namespace BinaryNinja
{
	/// <summary>
	/// Fallback wrapper for Medium Level IL operations that have no dedicated class
	/// yet (e.g. PASS_BY_REF, RETURN_BY_REF, STORE_OUTPUT, VAR_OUTPUT_*,
	/// BLOCK_TO_EXPAND, or operations added in a newer core than this binding).
	/// Generic navigation (operation, address, tokens, cross-level, SSA) still
	/// works; typed operand accessors are not exposed. Use
	/// <see cref="MediumLevelILInstruction.RawOperands"/> if needed.
	/// </summary>
	public sealed class MLILGeneric : MediumLevelILInstruction
	{
		internal MLILGeneric(
			MediumLevelILFunction ilFunction ,
			MediumLevelILExpressionIndex expressionIndex ,
			BNMediumLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}
	}
}
