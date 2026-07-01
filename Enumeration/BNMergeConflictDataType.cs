using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum MergeConflictDataType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		TextConflictDataType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		JsonConflictDataType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		BinaryConflictDataType = 2
	}
}