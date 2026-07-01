using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum UpdateResult : byte
	{
		/// <summary>
		/// 
		/// </summary>
		UpdateFailed = 0,
		
		/// <summary>
		/// 
		/// </summary>
		UpdateSuccess = 1,
		
		/// <summary>
		/// 
		/// </summary>
		AlreadyUpToDate = 2,
		
		/// <summary>
		/// 
		/// </summary>
		UpdateAvailable = 3
	}
}