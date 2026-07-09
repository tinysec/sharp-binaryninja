namespace BinaryNinja
{
	/// <summary>
	/// One row of the static detailed-operands table shared by every IL: an operand's name, the
	/// reader kind, the raw operand index it occupies, and the Python type string. Used to build
	/// <see cref="ILOperand"/> values at query time.
	/// </summary>
	internal readonly struct ILOperandDescriptor
	{
		internal string Name { get; }

		internal ILOperandKind Kind { get; }

		/// <summary>
		/// The raw operand slot the value starts at. For <see cref="ILOperandKind.SSAVariable"/>
		/// the variable identifier occupies this slot; for <see cref="ILOperandKind.ConstantData"/>
		/// the register-value state occupies this slot.
		/// </summary>
		internal int RawIndex { get; }

		/// <summary>
		/// The second raw slot a two-slot operand uses, or -1 to mean "the slot right after
		/// <see cref="RawIndex"/>". For <see cref="ILOperandKind.SSAVariable"/> this is the version
		/// slot; for <see cref="ILOperandKind.ConstantData"/> it is the value slot. The explicit
		/// form is needed for the MediumLevelIL "aliased" dest/prev operands, where <c>prev</c>
		/// shares the variable slot with <c>dest</c> but reads a different version slot (e.g. the
		/// core lays out <c>SET_VAR_ALIASED</c> as var@0, dest-version@1, prev-version@2).
		/// </summary>
		internal int SecondaryRawIndex { get; }

		internal string TypeName { get; }

		internal ILOperandDescriptor(string name, ILOperandKind kind, int rawIndex, string typeName)
		{
			this.Name = name;
			this.Kind = kind;
			this.RawIndex = rawIndex;
			this.SecondaryRawIndex = -1;
			this.TypeName = typeName;
		}

		internal ILOperandDescriptor(
			string name, ILOperandKind kind, int rawIndex, int secondaryRawIndex, string typeName)
		{
			this.Name = name;
			this.Kind = kind;
			this.RawIndex = rawIndex;
			this.SecondaryRawIndex = secondaryRawIndex;
			this.TypeName = typeName;
		}
	}
}
