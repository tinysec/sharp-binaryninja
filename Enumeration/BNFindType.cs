using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FindType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		FindTypeRawString = 0,
		
		/// <summary>
		/// 
		/// </summary>
		FindTypeEscapedString = 1,
		
		/// <summary>
		/// 
		/// </summary>
		FindTypeText = 2,
		
		/// <summary>
		/// 
		/// </summary>
		FindTypeConstant = 3,
		
		/// <summary>
		/// 
		/// </summary>
		FindTypeBytes = 4
	}
}