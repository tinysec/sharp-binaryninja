using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum DataFlowQueryOption : byte
	{
		/// <summary>
		/// 
		/// </summary>
		FromAddressesInLookupTableQueryOption = 0,
		
		/// <summary>
		/// 
		/// </summary>
		AllowReadingWritableMemoryQueryOption = 1
	}
}