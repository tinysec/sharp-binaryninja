using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FirmwareNinjaSectionAnalysisMode : byte
	{
		/// <summary>
		/// 
		/// </summary>
		DefaultSectionAnalysisMode = 0,
		
		/// <summary>
		/// 
		/// </summary>
		IgnorePaddingSectionAnalysisMode = 1,
		
		/// <summary>
		/// 
		/// </summary>
		DetectStringsSectionAnalysisMode = 2
	}
}