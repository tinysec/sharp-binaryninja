using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum FirmwareNinjaSectionType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		CodeSectionType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		DataSectionType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		CompressionSectionType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		PaddingSectionType = 3
	}
}