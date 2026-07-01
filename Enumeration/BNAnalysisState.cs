using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum AnalysisState : byte
	{
		/// <summary>
		/// 
		/// </summary>
		InitialState = 0,
		
		/// <summary>
		/// 
		/// </summary>
		HoldState = 1,
		
		/// <summary>
		/// 
		/// </summary>
		IdleState = 2,
		
		/// <summary>
		/// 
		/// </summary>
		DiscoveryState = 3,
		
		/// <summary>
		/// 
		/// </summary>
		DisassembleState = 4,
		
		/// <summary>
		/// 
		/// </summary>
		AnalyzeState = 5,
		
		/// <summary>
		/// 
		/// </summary>
		ExtendedAnalyzeState = 6
	}
}