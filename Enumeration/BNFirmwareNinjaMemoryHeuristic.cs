using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FirmwareNinjaMemoryHeuristic : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoMemoryHeuristic = 0,
		
		/// <summary>
		/// 
		/// </summary>
		HasReadBarrierMemoryHeuristic = 1,
		
		/// <summary>
		/// 
		/// </summary>
		HasWriteBarrierMemoryHeuristic = 2,
		
		/// <summary>
		/// 
		/// </summary>
		StoreToOOBMemoryMemoryHeuristic = 3,
		
		/// <summary>
		/// 
		/// </summary>
		LoadFromOOBMemoryMemoryHeuristic = 4,
		
		/// <summary>
		/// 
		/// </summary>
		RepeatLoadStoreMemoryHeuristic = 5,
		
		/// <summary>
		/// 
		/// </summary>
		CallParamOOBPointerMemoryHeuristic = 6
	}
}