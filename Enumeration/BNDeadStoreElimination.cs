using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum DeadStoreElimination : byte
	{
		/// <summary>
		/// 
		/// </summary>
		DefaultDeadStoreElimination = 0,
		
		/// <summary>
		/// 
		/// </summary>
		PreventDeadStoreElimination = 1,
		
		/// <summary>
		/// 
		/// </summary>
		AllowDeadStoreElimination = 2
	}
}