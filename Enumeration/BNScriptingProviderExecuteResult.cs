using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ScriptingProviderExecuteResult : byte
	{
		/// <summary>
		/// 
		/// </summary>
		InvalidScriptInput = 0,
		
		/// <summary>
		/// 
		/// </summary>
		IncompleteScriptInput = 1,
		
		/// <summary>
		/// 
		/// </summary>
		SuccessfulScriptExecution = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ScriptExecutionCancelled = 3
	}
}