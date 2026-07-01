using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum TypeParserErrorSeverity : byte
	{
		/// <summary>
		/// 
		/// </summary>
		IgnoredSeverity = 0,
		
		/// <summary>
		/// 
		/// </summary>
		NoteSeverity = 1,
		
		/// <summary>
		/// 
		/// </summary>
		RemarkSeverity = 2,
		
		/// <summary>
		/// 
		/// </summary>
		WarningSeverity = 3,
		
		/// <summary>
		/// 
		/// </summary>
		ErrorSeverity = 4,
		
		/// <summary>
		/// 
		/// </summary>
		FatalSeverity = 5
	}
}