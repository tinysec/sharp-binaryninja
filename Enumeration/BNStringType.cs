using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum StringType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		AsciiString = 0,
		
		/// <summary>
		/// 
		/// </summary>
		Utf16String = 1,
		
		/// <summary>
		/// 
		/// </summary>
		Utf32String = 2,
		
		/// <summary>
		/// 
		/// </summary>
		Utf8String = 3
	}
}