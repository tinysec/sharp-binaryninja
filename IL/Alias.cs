namespace BinaryNinja
{
	public enum IntrinsicIndex : uint
	{
		Invalid = 0xffffffff
	}
	
	public enum FlagIndex : uint
	{
		Invalid = 0xffffffff
	}
	
	public enum SemanticFlagClassIndex : uint
	{
		Invalid = 0xffffffff
	}
	
	public enum SemanticFlagGroupIndex : uint
	{
		Invalid = 0xffffffff
	}
	
	public enum RegisterIndex : uint
	{
		Invalid = 0xffffffff
	}
	
	public enum RegisterStackIndex : uint
	{
		Invalid = 0xffffffff
	}
	
	// An operand index is a 32-bit value; the core's invalid sentinel is
	// BN_INVALID_OPERAND (0xffffffff), matching the official Python binding
	// (architecture.py: operand = 0xffffffff). This stays 0xffffffff even though
	// the enum storage is 64-bit.
	public enum OperandIndex : ulong
	{
		Invalid = 0xffffffff
	}

	// IL expression and instruction indices are native size_t (64-bit). The core's
	// invalid sentinel is (size_t)-1 == BN_INVALID_EXPR == 0xffffffffffffffff, which
	// is what the official Python binding compares against (architecture.py:
	// il_expr_index = 0xffffffffffffffff; basicblock.py: instrIndex != 0xffffffffffffffff).
	// It must be the full 64-bit value, NOT the 32-bit 0xffffffff used for
	// register/operand indices, or a real core-returned invalid index would not match.
	public enum LowLevelILExpressionIndex : ulong
	{
		Invalid = 0xffffffffffffffff
	}

	public enum LowLevelILInstructionIndex : ulong
	{
		Invalid = 0xffffffffffffffff
	}
	
	public enum LowLevelILPossibleValueSetCacheIndex : ulong
	{
		Invalid = 0xffffffff
	}
	
	// See LowLevelILExpressionIndex: 64-bit (size_t)-1 invalid sentinel.
	public enum MediumLevelILExpressionIndex : ulong
	{
		Invalid = 0xffffffffffffffff
	}

	public enum MediumLevelILInstructionIndex : ulong
	{
		Invalid = 0xffffffffffffffff
	}
	
	public enum MediumLevelILPossibleValueSetCacheIndex : ulong
	{
		Invalid = 0xffffffff
	}
	
	// See LowLevelILExpressionIndex: 64-bit (size_t)-1 invalid sentinel.
	public enum HighLevelILExpressionIndex : ulong
	{
		Invalid = 0xffffffffffffffff
	}

	public enum HighLevelILInstructionIndex : ulong
	{
		Invalid = 0xffffffffffffffff
	}
	
	public enum HighLevelILPossibleValueSetCacheIndex : ulong
	{
		Invalid = 0xffffffff
	}

	public enum VariableIdentifier : ulong
	{
		Invalid = 0xffffffff
	}
}
