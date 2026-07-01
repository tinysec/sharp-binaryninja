using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum TypeDefinitionLineType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		TypedefLineType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		StructDefinitionLineType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		StructFieldLineType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		StructDefinitionEndLineType = 3,
		
		/// <summary>
		/// 
		/// </summary>
		EnumDefinitionLineType = 4,
		
		/// <summary>
		/// 
		/// </summary>
		EnumMemberLineType = 5,
		
		/// <summary>
		/// 
		/// </summary>
		EnumDefinitionEndLineType = 6,
		
		/// <summary>
		/// 
		/// </summary>
		PaddingLineType = 7,
		
		/// <summary>
		/// 
		/// </summary>
		UndefinedXrefLineType = 8,
		
		/// <summary>
		/// 
		/// </summary>
		CollapsedPaddingLineType = 9,
		
		/// <summary>
		/// 
		/// </summary>
		EmptyLineType = 10
	}
}