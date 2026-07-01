using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum SymbolType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		FunctionSymbol = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ImportAddressSymbol = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ImportedFunctionSymbol = 2,
		
		/// <summary>
		/// 
		/// </summary>
		DataSymbol = 3,
		
		/// <summary>
		/// 
		/// </summary>
		ImportedDataSymbol = 4,
		
		/// <summary>
		/// 
		/// </summary>
		ExternalSymbol = 5,
		
		/// <summary>
		/// 
		/// </summary>
		LibraryFunctionSymbol = 6,
		
		/// <summary>
		/// 
		/// </summary>
		SymbolicFunctionSymbol = 7,
		
		/// <summary>
		/// 
		/// </summary>
		LocalLabelSymbol = 8
	}
}