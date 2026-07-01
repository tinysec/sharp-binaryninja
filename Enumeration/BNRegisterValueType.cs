using System;

namespace BinaryNinja
{
	/// <summary>
	/// BNRegisterValueType
	/// </summary>
    public enum RegisterValueType : uint
	{
		UndeterminedValue,
		EntryValue,
		ConstantValue,
		ConstantPointerValue,
		ExternalPointerValue,
		StackFrameOffset,
		ReturnAddressValue,
		ImportedAddressValue,
		ResultPointerValue,
		ParameterPointerValue,
		SignedRangeValue,
		UnsignedRangeValue,
		LookupTableValue,
		InSetOfValues,
		NotInSetOfValues,
		ConstantDataValue = 0x8000,
		ConstantDataZeroExtendValue = ConstantDataValue | 0x1,
		ConstantDataSignExtendValue = ConstantDataValue | 0x2,
		ConstantDataAggregateValue = ConstantDataValue | 0x3
	}
}
