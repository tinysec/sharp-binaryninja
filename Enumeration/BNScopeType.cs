using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ScopeType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		OneLineScopeType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		HasSubScopeScopeType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		BlockScopeType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		SwitchScopeType = 3,
		
		/// <summary>
		/// 
		/// </summary>
		CaseScopeType = 4
	}
}