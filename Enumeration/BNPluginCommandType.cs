using System;

namespace BinaryNinja
{
	/// <summary>
	/// BNPluginCommandType
	/// </summary>
    public enum PluginCommandType : byte
	{
		DefaultPluginCommand,
		AddressPluginCommand,
		RangePluginCommand,
		FunctionPluginCommand,
		LowLevelILFunctionPluginCommand,
		LowLevelILInstructionPluginCommand,
		MediumLevelILFunctionPluginCommand,
		MediumLevelILInstructionPluginCommand,
		HighLevelILFunctionPluginCommand,
		HighLevelILInstructionPluginCommand,
		ProjectPluginCommand,
		GlobalPluginCommand
	}
}
