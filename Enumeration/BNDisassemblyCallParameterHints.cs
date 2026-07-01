using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum DisassemblyCallParameterHints : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NeverShowMatchingParameterHints = 0,
		
		/// <summary>
		/// 
		/// </summary>
		AlwaysShowParameterHints = 1,
		
		/// <summary>
		/// 
		/// </summary>
		NeverShowParameterHints = 2
	}
}