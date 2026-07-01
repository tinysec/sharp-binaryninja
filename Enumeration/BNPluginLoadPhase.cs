using System;

namespace BinaryNinja
{
	/// <summary>
	///
	/// </summary>
    public enum PluginLoadPhase : byte
	{
		/// <summary>
		///
		/// </summary>
		NativePluginLoadPhase = 0,

		/// <summary>
		///
		/// </summary>
		ScriptingProviderLoadPhase = 1,

		/// <summary>
		///
		/// </summary>
		ScriptPluginLoadPhase = 2
	}
}
