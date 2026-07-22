namespace BinaryNinja
{
	/// <summary>
	/// A named, typed operand of an IL instruction: the value plus its name, kind, and the Python
	/// type string. Shared by every IL. This is the C# equivalent of one
	/// <c>(name, value, type_str)</c> tuple from the official Python binding's
	/// <c>*ILInstruction.detailed_operands</c> property.
	/// </summary>
	public readonly struct ILOperand
	{
		/// <summary>The operand's name, e.g. "left", "src", "condition" (matches Python).</summary>
		public string Name { get; }

		/// <summary>
		/// The operand value. The runtime type follows <see cref="Kind"/>: an IL instruction,
		/// <c>long</c>, a Variable/SSAVariable, <c>float</c>, a PossibleValueSet, or an array of
		/// those. May be null when the underlying native operand is absent.
		/// </summary>
		public object? Value { get; }

		/// <summary>How the value was read; drives traversal and variable collection.</summary>
		public ILOperandKind Kind { get; }

		/// <summary>The Python type string, e.g. "*ILInstruction", "int", "Variable".</summary>
		public string TypeName { get; }

		internal ILOperand(string name, object? value, ILOperandKind kind, string typeName)
		{
			this.Name = name;
			this.Value = value;
			this.Kind = kind;
			this.TypeName = typeName;
		}
	}
}
