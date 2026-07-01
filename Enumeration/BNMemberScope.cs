using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum MemberScope : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoScope = 0,
		
		/// <summary>
		/// 
		/// </summary>
		StaticScope = 1,
		
		/// <summary>
		/// 
		/// </summary>
		VirtualScope = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ThunkScope = 3,
		
		/// <summary>
		/// 
		/// </summary>
		FriendScope = 4
	}
}