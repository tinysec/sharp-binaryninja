using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum SymbolBinding : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoBinding = 0,
		
		/// <summary>
		/// 
		/// </summary>
		LocalBinding = 1,
		
		/// <summary>
		/// 
		/// </summary>
		GlobalBinding = 2,
		
		/// <summary>
		/// 
		/// </summary>
		WeakBinding = 3
	}
}