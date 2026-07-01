using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ReferenceType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		PointerReferenceType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ReferenceReferenceType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		RValueReferenceType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		NoReference = 3
	}
}