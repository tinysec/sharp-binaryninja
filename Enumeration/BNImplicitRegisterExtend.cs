using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum ImplicitRegisterExtend : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoExtend = 0,
		
		/// <summary>
		/// 
		/// </summary>
		ZeroExtendToFullWidth = 1,
		
		/// <summary>
		/// 
		/// </summary>
		SignExtendToFullWidth = 2
	}
}