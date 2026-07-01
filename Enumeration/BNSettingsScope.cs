using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum SettingsScope : byte
	{
		/// <summary>
		/// 
		/// </summary>
		SettingsInvalidScope = 0,
		
		/// <summary>
		/// 
		/// </summary>
		SettingsAutoScope = 1,
		
		/// <summary>
		/// 
		/// </summary>
		SettingsDefaultScope = 2,
		
		/// <summary>
		/// 
		/// </summary>
		SettingsUserScope = 4,
		
		/// <summary>
		/// 
		/// </summary>
		SettingsProjectScope = 8,
		
		/// <summary>
		/// 
		/// </summary>
		SettingsResourceScope = 16
	}
}