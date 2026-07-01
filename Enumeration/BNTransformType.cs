using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum TransformType : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		BinaryCodecTransform = 0,
		
		/// <summary>
		/// 
		/// </summary>
		TextCodecTransform = 1,
		
		/// <summary>
		/// 
		/// </summary>
		UnicodeCodecTransform = 2,
		
		/// <summary>
		/// 
		/// </summary>
		DecodeTransform = 3,
		
		/// <summary>
		/// 
		/// </summary>
		BinaryEncodeTransform = 4,
		
		/// <summary>
		/// 
		/// </summary>
		TextEncodeTransform = 5,
		
		/// <summary>
		/// 
		/// </summary>
		EncryptTransform = 6,
		
		/// <summary>
		/// 
		/// </summary>
		InvertingTransform = 7,
		
		/// <summary>
		/// 
		/// </summary>
		HashTransform = 8
	}
}