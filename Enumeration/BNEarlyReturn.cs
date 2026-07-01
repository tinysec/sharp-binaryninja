using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum EarlyReturn : byte
	{
		/// <summary>
		/// 
		/// </summary>
		DefaultEarlyReturn = 0,
		
		/// <summary>
		/// 
		/// </summary>
		PreventEarlyReturn = 1,
		
		/// <summary>
		/// 
		/// </summary>
		SmallestSideEarlyReturn = 2,
		
		/// <summary>
		/// 
		/// </summary>
		TrueSideEarlyReturn = 3,
		
		/// <summary>
		/// 
		/// </summary>
		FalseSideEarlyReturn = 4
	}
}