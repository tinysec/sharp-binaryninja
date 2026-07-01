using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum SyncStatus : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NotSyncedSyncStatus = 0,
		
		/// <summary>
		/// 
		/// </summary>
		NoChangesSyncStatus = 1,
		
		/// <summary>
		/// 
		/// </summary>
		UnknownSyncStatus = 2,
		
		/// <summary>
		/// 
		/// </summary>
		CanPushSyncStatus = 3,
		
		/// <summary>
		/// 
		/// </summary>
		CanPullSyncStatus = 4,
		
		/// <summary>
		/// 
		/// </summary>
		CanPushAndPullSyncStatus = 5,
		
		/// <summary>
		/// 
		/// </summary>
		ConflictSyncStatus = 6
	}
}