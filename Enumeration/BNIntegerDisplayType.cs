using System;

namespace BinaryNinja
{
	/// <summary>
	/// BNIntegerDisplayType
	/// </summary>
    public enum IntegerDisplayType : byte
	{
		DefaultIntegerDisplayType,
		BinaryDisplayType,
		SignedOctalDisplayType,
		UnsignedOctalDisplayType,
		SignedDecimalDisplayType,
		UnsignedDecimalDisplayType,
		SignedHexadecimalDisplayType,
		UnsignedHexadecimalDisplayType,
		CharacterConstantDisplayType,
		PointerDisplayType,
		FloatDisplayType,
		DoubleDisplayType,
		EnumerationDisplayType,
		InvertedCharacterConstantDisplayType,
		UnsignedComplementDecimalDisplayType,
		UnsignedComplementHexadecimalDisplayType
	}
}
