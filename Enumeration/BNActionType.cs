using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ActionType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		TemporaryAction = 0,
		
		/// <summary>
		/// 
		/// </summary>
		DataModificationAction = 1,
		
		/// <summary>
		/// 
		/// </summary>
		AnalysisAction = 2,
		
		/// <summary>
		/// 
		/// </summary>
		DataModificationAndAnalysisAction = 3
	}
}