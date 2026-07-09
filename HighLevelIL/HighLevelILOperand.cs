namespace BinaryNinja
{
	/// <summary>
	/// Categorizes one detailed operand of a <see cref="HighLevelILInstruction"/> by how it is read
	/// from the native instruction's raw operand slots. Mirrors the type strings returned by Python's
	/// <c>HighLevelILInstruction.detailed_operands</c> (highlevelil.py), collapsed to the reader
	/// dispatched in <see cref="HighLevelILInstruction.DetailedOperands"/>.
	/// </summary>
	public enum HighLevelILOperandKind
	{
		/// <summary>A single sub-expression (Python type "HighLevelILInstruction").</summary>
		Expression,

		/// <summary>A list of sub-expressions (Python type "List[HighLevelILInstruction]").</summary>
		ExpressionList,

		/// <summary>A single integer slot (Python type "int" or "Optional[int]").</summary>
		Integer,

		/// <summary>A list of integer slots (Python type "List[int]").</summary>
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

		/// <summary>A goto label reference (Python type "GotoLabel").</summary>
		GotoLabel,

		/// <summary>Constant data (Python type "ConstantData").</summary>
		ConstantData,

		/// <summary>An intrinsic reference (Python type "ILIntrinsic").</summary>
		Intrinsic
	}

	/// <summary>
	/// A named, typed operand of a <see cref="HighLevelILInstruction"/>: the value plus its name,
	/// kind, and the Python type string. This is the C# equivalent of one
	/// <c>(name, value, type_str)</c> tuple from Python
	/// <c>HighLevelILInstruction.detailed_operands</c> (highlevelil.py:795).
	/// </summary>
	public readonly struct HighLevelILOperand
	{
		/// <summary>The operand's name, e.g. "left", "src", "condition" (matches Python).</summary>
		public string Name { get; }

		/// <summary>
		/// The operand value. The runtime type follows <see cref="Kind"/>: a
		/// <see cref="HighLevelILInstruction"/>, <c>long</c>, <see cref="HighLevelILVariable"/>,
		/// <see cref="HighLevelILSSAVariable"/>, <c>float</c>, or an array of those. May be null
		/// when the underlying native operand is absent.
		/// </summary>
		public object? Value { get; }

		/// <summary>How the value was read; drives traversal and variable collection.</summary>
		public HighLevelILOperandKind Kind { get; }

		/// <summary>The Python type string, e.g. "HighLevelILInstruction", "int", "Variable".</summary>
		public string TypeName { get; }

		internal HighLevelILOperand(string name, object? value, HighLevelILOperandKind kind, string typeName)
		{
			this.Name = name;
			this.Value = value;
			this.Kind = kind;
			this.TypeName = typeName;
		}
	}

	/// <summary>
	/// One row of the static detailed-operands table: an operand's name, the reader kind, the raw
	/// operand index it occupies, and the Python type string. Used to build
	/// <see cref="HighLevelILOperand"/> values at query time.
	/// </summary>
	internal readonly struct HighLevelILOperandDescriptor
	{
		internal string Name { get; }

		internal HighLevelILOperandKind Kind { get; }

		/// <summary>
		/// The raw operand slot the value starts at. For <see cref="HighLevelILOperandKind.SSAVariable"/>
		/// the version occupies the next slot (index + 1).
		/// </summary>
		internal int RawIndex { get; }

		internal string TypeName { get; }

		internal HighLevelILOperandDescriptor(
			string name, HighLevelILOperandKind kind, int rawIndex, string typeName)
		{
			this.Name = name;
			this.Kind = kind;
			this.RawIndex = rawIndex;
			this.TypeName = typeName;
		}
	}
}
