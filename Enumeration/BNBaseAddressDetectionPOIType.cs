using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum BaseAddressDetectionPOIType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		POIString = 0,
		
		/// <summary>
		/// 
		/// </summary>
		POIFunction = 1,
		
		/// <summary>
		/// 
		/// </summary>
		POIDataVariable = 2,
		
		/// <summary>
		/// 
		/// </summary>
		POIFileStart = 3,
		
		/// <summary>
		/// 
		/// </summary>
		POIFileEnd = 4
	}
}