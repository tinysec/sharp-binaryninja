using System;

namespace BinaryNinja
{
	/// <summary>
	/// BNVariableSourceType
	/// </summary>
    public enum VariableSourceType : byte
	{
		StackVariableSourceType,
		RegisterVariableSourceType,
		FlagVariableSourceType,
		CompositeReturnValueSourceType,
		CompositeParameterSourceType
	}
}
