using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum SegmentFlag : byte
	{
		/// <summary>
		/// 
		/// </summary>
		SegmentExecutable = 1,
		
		/// <summary>
		/// 
		/// </summary>
		SegmentWritable = 2,
		
		/// <summary>
		/// 
		/// </summary>
		SegmentReadable = 4,
		
		/// <summary>
		/// 
		/// </summary>
		SegmentContainsData = 8,
		
		/// <summary>
		/// 
		/// </summary>
		SegmentContainsCode = 16,
		
		/// <summary>
		/// 
		/// </summary>
		SegmentDenyWrite = 32,
		
		/// <summary>
		/// 
		/// </summary>
		SegmentDenyExecute = 64
	}
}