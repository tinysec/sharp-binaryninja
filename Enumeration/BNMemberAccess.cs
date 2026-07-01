using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum MemberAccess : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoAccess = 0,
		
		/// <summary>
		/// 
		/// </summary>
		PrivateAccess = 1,
		
		/// <summary>
		/// 
		/// </summary>
		ProtectedAccess = 2,
		
		/// <summary>
		/// 
		/// </summary>
		PublicAccess = 3
	}
}