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

		/// <summary>An intrinsic reference (Python type "ILIntrinsic").</summary>
		Intrinsic
	}
}
