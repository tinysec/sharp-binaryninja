using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum TransformCapabilities : byte
	{
		/// <summary>
		/// 
		/// </summary>
		TransformNoCapabilities = 0,
		
		/// <summary>
		/// 
		/// </summary>
		TransformSupportsDetection = 1,
		
		/// <summary>
		/// 
		/// </summary>
		TransformSupportsContext = 2
	}
}