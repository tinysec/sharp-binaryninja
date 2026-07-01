using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum PointerBaseType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		AbsolutePointerBaseType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		RelativeToConstantPointerBaseType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		RelativeToBinaryStartPointerBaseType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		RelativeToVariableAddressPointerBaseType = 3
	}
}