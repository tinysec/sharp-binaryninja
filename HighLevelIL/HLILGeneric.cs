namespace BinaryNinja
{
	/// <summary>
	/// Fallback wrapper for High Level IL operations that have no dedicated class
	/// yet (e.g. PASS_BY_REF, RETURN_BY_REF, VAR_SSA_PARTIAL, or operations added
	/// in a newer core than this binding). Generic navigation (operation, address,
	/// tokens, cross-level, SSA) still works; typed operand accessors are not
	/// exposed. Use <see cref="HighLevelILInstruction.RawOperands"/> if needed.
	/// </summary>
	public sealed class HLILGeneric : HighLevelILInstruction
	{
		internal HLILGeneric(
			HighLevelILFunction ilFunction ,
			HighLevelILExpressionIndex expressionIndex ,
			BNHighLevelILInstruction native
		) : base(ilFunction , expressionIndex , native)
		{

		}
	}
}
