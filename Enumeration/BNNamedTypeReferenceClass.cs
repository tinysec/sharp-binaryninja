using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum NamedTypeReferenceClass : byte
	{
		/// <summary>
		/// 
		/// </summary>
		UnknownNamedTypeClass = 0,
		
		/// <summary>
		/// 
		/// </summary>
		TypedefNamedTypeClass = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ClassNamedTypeClass = 2,
		
		/// <summary>
		/// 
		/// </summary>
		StructNamedTypeClass = 3,
		
		/// <summary>
		/// 
		/// </summary>
		UnionNamedTypeClass = 4,
		
		/// <summary>
		/// 
		/// </summary>
		EnumNamedTypeClass = 5
	}
}