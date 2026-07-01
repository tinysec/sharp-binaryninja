using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum PluginType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		CorePluginType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		UiPluginType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ArchitecturePluginType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		BinaryViewPluginType = 3,
		
		/// <summary>
		/// 
		/// </summary>
		HelperPluginType = 4,
		
		/// <summary>
		/// 
		/// </summary>
		SyncPluginType = 5
	}
}