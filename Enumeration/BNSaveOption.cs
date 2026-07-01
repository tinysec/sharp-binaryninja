using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum SaveOption : byte
	{
		/// <summary>
		/// 
		/// </summary>
		RemoveUndoData = 0,
		
		/// <summary>
		/// 
		/// </summary>
		TrimSnapshots = 1,
		
		/// <summary>
		/// 
		/// </summary>
		PurgeOriginalFilenamePath = 2
	}
}