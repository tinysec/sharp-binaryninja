using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum BraceRequirement : byte
	{
		/// <summary>
		/// 
		/// </summary>
		OptionalBraces = 0,
		
		/// <summary>
		/// 
		/// </summary>
		BracesNotAllowed = 1,
		
		/// <summary>
		/// 
		/// </summary>
		BracesAlwaysRequired = 2
	}
}