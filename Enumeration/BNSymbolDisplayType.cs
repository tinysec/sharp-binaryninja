using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum SymbolDisplayType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		DisplaySymbolOnly = 0,
		
		/// <summary>
		/// 
		/// </summary>
		AddressOfDataSymbols = 1,
		
		/// <summary>
		/// 
		/// </summary>
		DereferenceNonDataSymbols = 2
	}
}