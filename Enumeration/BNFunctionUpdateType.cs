using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FunctionUpdateType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		UserFunctionUpdate = 0,
		
		/// <summary>
		/// 
		/// </summary>
		FullAutoFunctionUpdate = 1,
		
		/// <summary>
		/// 
		/// </summary>
		IncrementalAutoFunctionUpdate = 2
	}
}