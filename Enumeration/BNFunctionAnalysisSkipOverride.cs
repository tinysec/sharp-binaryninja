using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FunctionAnalysisSkipOverride : byte
	{
		/// <summary>
		/// 
		/// </summary>
		DefaultFunctionAnalysisSkip = 0,
		
		/// <summary>
		/// 
		/// </summary>
		NeverSkipFunctionAnalysis = 1,
		
		/// <summary>
		/// 
		/// </summary>
		AlwaysSkipFunctionAnalysis = 2
	}
}