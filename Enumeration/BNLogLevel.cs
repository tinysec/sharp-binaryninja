using System;

namespace BinaryNinja
{
	/// <summary>
	/// Console log levels
	/// </summary>
    public enum LogLevel : byte
	{
		/// <summary>
		/// 
		/// </summary>
		DebugLog = 0,
		
		/// <summary>
		/// Debug logging level, most verbose logging level
		/// </summary>
		InfoLog = 1,
		
		/// <summary>
		/// Information logging level, default logging level
		/// </summary>
		WarningLog = 2,
		
		/// <summary>
		/// Warning logging level, messages show with warning icon in the UI
		/// </summary>
		ErrorLog = 3,
		
		/// <summary>
		/// Error logging level, messages show with error icon in the UI
		/// </summary>
		AlertLog = 4
	}
}