using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum DisassemblyOption : byte
	{
		/// <summary>
		/// 
		/// </summary>
		ShowAddress = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ShowOpcode = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ExpandLongOpcode = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ShowVariablesAtTopOfGraph = 3,
		
		/// <summary>
		/// 
		/// </summary>
		ShowVariableTypesWhenAssigned = 4,
		
		/// <summary>
		/// 
		/// </summary>
		ShowRegisterHighlight = 7,
		
		/// <summary>
		/// 
		/// </summary>
		ShowFunctionAddress = 8,
		
		/// <summary>
		/// 
		/// </summary>
		ShowFunctionHeader = 9,
		
		/// <summary>
		/// 
		/// </summary>
		ShowTypeCasts = 10,
		
		/// <summary>
		/// 
		/// </summary>
		GroupLinearDisassemblyFunctions = 64,
		
		/// <summary>
		/// 
		/// </summary>
		HighLevelILLinearDisassembly = 65,
		
		/// <summary>
		/// 
		/// </summary>
		WaitForIL = 66,
		
		/// <summary>
		/// 
		/// </summary>
		IndentHLILBody = 67,
		
		/// <summary>
		/// 
		/// </summary>
		DisableLineFormatting = 68,
		
		/// <summary>
		/// 
		/// </summary>
		ShowFlagUsage = 128,
		
		/// <summary>
		/// 
		/// </summary>
		ShowStackPointer = 129,
		
		/// <summary>
		/// 
		/// </summary>
		ShowILTypes = 130,
		
		/// <summary>
		/// 
		/// </summary>
		ShowILOpcodes = 131,
		
		/// <summary>
		/// 
		/// </summary>
		ShowCollapseIndicators = 132
	}
}