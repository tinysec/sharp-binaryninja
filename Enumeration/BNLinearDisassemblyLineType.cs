using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum LinearDisassemblyLineType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		BlankLineType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		BasicLineType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		CodeDisassemblyLineType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		DataVariableLineType = 3,
		
		/// <summary>
		/// 
		/// </summary>
		HexDumpLineType = 4,
		
		/// <summary>
		/// 
		/// </summary>
		FunctionHeaderLineType = 5,
		
		/// <summary>
		/// 
		/// </summary>
		FunctionHeaderStartLineType = 6,
		
		/// <summary>
		/// 
		/// </summary>
		FunctionHeaderEndLineType = 7,
		
		/// <summary>
		/// 
		/// </summary>
		FunctionContinuationLineType = 8,
		
		/// <summary>
		/// 
		/// </summary>
		LocalVariableLineType = 9,
		
		/// <summary>
		/// 
		/// </summary>
		LocalVariableListEndLineType = 10,
		
		/// <summary>
		/// 
		/// </summary>
		FunctionEndLineType = 11,
		
		/// <summary>
		/// 
		/// </summary>
		NoteStartLineType = 12,
		
		/// <summary>
		/// 
		/// </summary>
		NoteLineType = 13,
		
		/// <summary>
		/// 
		/// </summary>
		NoteEndLineType = 14,
		
		/// <summary>
		/// 
		/// </summary>
		SectionStartLineType = 15,
		
		/// <summary>
		/// 
		/// </summary>
		SectionEndLineType = 16,
		
		/// <summary>
		/// 
		/// </summary>
		SectionSeparatorLineType = 17,
		
		/// <summary>
		/// 
		/// </summary>
		NonContiguousSeparatorLineType = 18,
		
		/// <summary>
		/// 
		/// </summary>
		AnalysisWarningLineType = 19,
		
		/// <summary>
		/// 
		/// </summary>
		CollapsedFunctionEndLineType = 20
	}
}