using System;

namespace BinaryNinja
{
	/// <summary>
	/// BNInstructionTextTokenContext
	/// </summary>
    public enum InstructionTextTokenContext : byte
	{
		NoTokenContext = 0,
		LocalVariableTokenContext = 1,
		DataVariableTokenContext = 2,
		FunctionReturnTokenContext = 3,
		InstructionAddressTokenContext = 4,
		ILInstructionIndexTokenContext = 5,
		ConstDataTokenContext = 6,
		ConstStringDataTokenContext = 7,
		StringReferenceTokenContext = 8,
		StringDataVariableTokenContext = 9,
		StringDisplayTokenContext = 10,
		ContentCollapsedContext = 11,
		ContentExpandedContext = 12,
		ContentCollapsiblePadding = 13,
		DerivedStringReferenceTokenContext = 14
	}
}
