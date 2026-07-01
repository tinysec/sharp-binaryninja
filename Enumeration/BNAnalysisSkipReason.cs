using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum AnalysisSkipReason : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoSkipReason = 0,
		
		/// <summary>
		/// 
		/// </summary>
		AlwaysSkipReason = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ExceedFunctionSizeSkipReason = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ExceedFunctionAnalysisTimeSkipReason = 3,
		
		/// <summary>
		/// 
		/// </summary>
		ExceedFunctionUpdateCountSkipReason = 4,
		
		/// <summary>
		/// 
		/// </summary>
		NewAutoFunctionAnalysisSuppressedReason = 5,
		
		/// <summary>
		/// 
		/// </summary>
		BasicAnalysisSkipReason = 6,
		
		/// <summary>
		/// 
		/// </summary>
		IntermediateAnalysisSkipReason = 7,
		
		/// <summary>
		/// 
		/// </summary>
		AnalysisPipelineSuspendedReason = 8
	}
}