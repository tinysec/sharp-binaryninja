using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum TokenEscapingType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoTokenEscapingType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		BackticksTokenEscapingType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		QuotedStringEscapingType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ReplaceInvalidCharsEscapingType = 3
	}
}