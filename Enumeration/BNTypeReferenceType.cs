using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum TypeReferenceType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		DirectTypeReferenceType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		IndirectTypeReferenceType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		UnknownTypeReferenceType = 2
	}
}