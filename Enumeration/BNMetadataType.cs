using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum MetadataType : byte
	{
		/// <summary>
		/// 
		/// </summary>
		InvalidDataType = 0,
		
		/// <summary>
		/// 
		/// </summary>
		BooleanDataType = 1,
		
		/// <summary>
		/// 
		/// </summary>
		StringDataType = 2,
		
		/// <summary>
		/// 
		/// </summary>
		UnsignedIntegerDataType = 3,
		
		/// <summary>
		/// 
		/// </summary>
		SignedIntegerDataType = 4,
		
		/// <summary>
		/// 
		/// </summary>
		DoubleDataType = 5,
		
		/// <summary>
		/// 
		/// </summary>
		RawDataType = 6,
		
		/// <summary>
		/// 
		/// </summary>
		KeyValueDataType = 7,
		
		/// <summary>
		/// 
		/// </summary>
		ArrayDataType = 8
	}
}