using System;

namespace BinaryNinja
{
	/// <summary>
	/// 
	/// </summary>
    public enum CallingConventionName : byte
	{
		/// <summary>
		/// 
		/// </summary>
		NoCallingConvention = 0,
		
		/// <summary>
		/// 
		/// </summary>
		CdeclCallingConvention = 1,
		
		/// <summary>
		/// 
		/// </summary>
		PascalCallingConvention = 2,
		
		/// <summary>
		/// 
		/// </summary>
		ThisCallCallingConvention = 3,
		
		/// <summary>
		/// 
		/// </summary>
		STDCallCallingConvention = 4,
		
		/// <summary>
		/// 
		/// </summary>
		FastcallCallingConvention = 5,
		
		/// <summary>
		/// 
		/// </summary>
		CLRCallCallingConvention = 6,
		
		/// <summary>
		/// 
		/// </summary>
		EabiCallCallingConvention = 7,
		
		/// <summary>
		/// 
		/// </summary>
		VectorCallCallingConvention = 8,
		
		/// <summary>
		/// 
		/// </summary>
		SwiftCallingConvention = 9,
		
		/// <summary>
		/// 
		/// </summary>
		SwiftAsyncCallingConvention = 10
	}
}