using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum AnalysisMode : byte
	{
		/// <summary>
		/// 
		/// </summary>
		FullAnalysisMode = 0,
		
		/// <summary>
		/// 
		/// </summary>
		IntermediateAnalysisMode = 1,
		
		/// <summary>
		/// 
		/// </summary>
		BasicAnalysisMode = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ControlFlowAnalysisMode = 3
	}
}