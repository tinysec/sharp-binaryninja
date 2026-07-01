using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum PointerSuffix : byte
	{
		/// <summary>
		/// 
		/// </summary>
		Ptr64Suffix = 0,
		
		/// <summary>
		/// 
		/// </summary>
		UnalignedSuffix = 1,
		
		/// <summary>
		/// 
		/// </summary>
		RestrictSuffix = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ReferenceSuffix = 3,
		
		/// <summary>
		/// 
		/// </summary>
		LvalueSuffix = 4
	}
}