using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum BuiltinType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		BuiltinNone = 0,
		
		/// <summary>
		/// 
		/// </summary>
		BuiltinMemcpy = 1,
		
		/// <summary>
		/// 
		/// </summary>
		BuiltinMemset = 2,
		
		/// <summary>
		/// 
		/// </summary>
		BuiltinStrncpy = 3,
		
		/// <summary>
		/// 
		/// </summary>
		BuiltinStrcpy = 4,
		
		/// <summary>
		/// 
		/// </summary>
		BuiltinWcscpy = 5,
		
		/// <summary>
		/// 
		/// </summary>
		BuiltinWmemcpy = 6
	}
}