using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ScriptingProviderInputReadyState : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NotReadyForInput = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ReadyForScriptExecution = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ReadyForScriptProgramInput = 2
	}
}