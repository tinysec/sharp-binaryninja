namespace BinaryNinja
{
	/// <summary>
	/// Categorizes one detailed operand of an IL instruction by how it is read from the native
	/// instruction's raw operand slots. Shared by every IL (HighLevelIL, MediumLevelIL, ...) so the
	/// descriptor tables, traversal, and operand collection can be written once.
	/// </summary>
	/// <remarks>
	/// Each value mirrors a type string returned by the official Python binding's
	/// <c>*ILInstruction.detailed_operands</c> property, collapsed to the reader dispatched in the
	/// matching <c>DetailedOperands</c> implementation.
	/// </remarks>
	public enum ILOperandKind
	{
		/// <summary>A single sub-expression (Python type "*ILInstruction").</summary>
		Expression,

		/// <summary>A list of sub-expressions (Python type "List[*ILInstruction]").</summary>
		ExpressionList,

		/// <summary>A single integer slot (Python type "int" or "Optional[int]").</summary>
		Integer,

		/// <summary>A list of integer slots (Python type "List[int]" or "Dict[int, int]").</summary>
		IntegerList,

		/// <summary>A single variable (Python type "Variable").</summary>
		Variable,

		/// <summary>A list of variables (Python type "List[Variable]").</summary>
		VariableList,

		/// <summary>A single SSA variable occupying two raw slots (Python type "SSAVariable").</summary>
		SSAVariable,

		/// <summary>A list of SSA variables (Python type "List[SSAVariable]").</summary>
		SSAVariableList,

		/// <summary>A floating-point constant (Python type "float").</summary>
		Float,

		/// <summary>A goto label reference (Python type "GotoLabel"). HighLevelIL only.</summary>
		GotoLabel,

		/// <summary>Constant data (Python type "ConstantData").</summary>
		ConstantData,

		/// <summary>A cached possible-value constraint (Python type "PossibleValueSet").</summary>
		PossibleValueSet,

		/// <summary>An intrinsic reference (Python type "ILIntrinsic").</summary>
		Intrinsic,

		// --- MediumLevelIL sub-instruction-derived operands ---
		//
		// The eight kinds below serve operands whose VALUE lives inside a nested sub-instruction
		// expression (a CallOutput / CallParam / MemoryIntrinsicOutput wrapper) rather than in a
		// flat raw slot of the enclosing instruction. They are read by fetching the sub-instruction
		// at <see cref="ILOperandDescriptor.RawIndex"/> (GetOperandAsExpression) and returning one
		// of its existing properties. Each kind names the sub-instruction class it casts to and the
		// property it reads, so the dispatch in MediumLevelILInstruction.ReadDetailedOperandByKind
		// stays a flat switch with one case per kind. HighLevelIL never produces these (its call
		// operands are flat lists), so they carry no HighLevelIL reader.

		/// <summary>
		/// The output variables of a non-SSA call/syscall/tailcall, reached by reading the
		/// MLILCallOutput sub-instruction's Destination (Python "List[Variable]").
		/// </summary>
		CallOutputVariables,

		/// <summary>
		/// The parameter expressions of a non-SSA call/syscall/tailcall, reached by reading the
		/// MLILCallParam sub-instruction's Source (Python "List[MediumLevelILInstruction]").
		/// </summary>
		CallParamExpressions,

		/// <summary>
		/// The output SSA variables of an SSA call/syscall/tailcall, reached by reading the
		/// MLILCallOutputSSA sub-instruction's Destination (Python "List[SSAVariable]").
		/// </summary>
		CallOutputSsaVariables,

		/// <summary>
		/// The output destination-memory version of an SSA call/syscall/tailcall, reached by
		/// reading the MLILCallOutputSSA sub-instruction's DestinationMemory (Python "int").
		/// </summary>
		CallOutputSsaMemory,

		/// <summary>
		/// The parameter expressions of an untyped SSA call/syscall/tailcall, reached by reading
		/// the MLILCallParamSSA sub-instruction's Source (Python "List[MediumLevelILInstruction]").
		/// </summary>
		CallParamSsaExpressions,

		/// <summary>
		/// The parameter source-memory version of an untyped SSA call/syscall, reached by reading
		/// the MLILCallParamSSA sub-instruction's SourceMemory (Python "int").
		/// </summary>
		CallParamSsaMemory,

		/// <summary>
		/// The output SSA variables of MLIL_MEMORY_INTRINSIC_SSA, reached by reading the
		/// MLILMemoryIntrinsicOutputSSA sub-instruction's Output (Python "List[SSAVariable]").
		/// </summary>
		MemoryIntrinsicOutputSsaVariables,

		/// <summary>
		/// The destination-memory version of MLIL_MEMORY_INTRINSIC_SSA, reached by reading the
		/// MLILMemoryIntrinsicOutputSSA sub-instruction's DestinationMemory (Python "int").
		/// </summary>
		MemoryIntrinsicOutputSsaMemory,

		// --- LowLevelIL operands ---
		// These values remain after the older MLIL kinds to preserve their public numeric values.

		/// <summary>An architecture register (Python type "ILRegister").</summary>
		Register,

		/// <summary>An architecture flag (Python type "ILFlag").</summary>
		Flag,

		/// <summary>An architecture register stack (Python type "ILRegisterStack").</summary>
		RegisterStack,

		/// <summary>An SSA register occupying two raw slots (Python type "SSARegister").</summary>
		SSARegister,

		/// <summary>An SSA flag occupying two raw slots (Python type "SSAFlag").</summary>
		SSAFlag,

		/// <summary>An SSA register stack occupying two raw slots.</summary>
		SSARegisterStack,

		/// <summary>A low-level flag condition.</summary>
		FlagCondition,

		/// <summary>An architecture semantic flag class.</summary>
		SemanticFlagClass,

		/// <summary>An architecture semantic flag group.</summary>
		SemanticFlagGroup,

		/// <summary>A jump-table target map.</summary>
		TargetMap,

		/// <summary>Register-stack adjustments associated with a call.</summary>
		RegisterStackAdjustments,

		/// <summary>A list containing architecture registers and flags.</summary>
		RegisterOrFlagList,

		/// <summary>A list of SSA registers.</summary>
		SSARegisterList,

		/// <summary>A list of SSA flags.</summary>
		SSAFlagList,

		/// <summary>A list of SSA register stacks.</summary>
		SSARegisterStackList,

		/// <summary>A list containing SSA registers and SSA flags.</summary>
		SSARegisterOrFlagList,

		// LowLevelIL values derived from nested call and intrinsic wrapper expressions.
		LLILCallOutputSsaRegisters,
		LLILCallStackSsaRegister,
		LLILCallStackSsaMemory,
		LLILCallParamExpressions,
		LLILMemoryIntrinsicOutputSsaRegisters,
		LLILMemoryIntrinsicOutputSsaMemory,

		// LLIL_ADD_OVERFLOW stores its left expression in the native flags field.
		LLILAddOverflowLeft,
		LLILAddOverflowRight
	}
}
