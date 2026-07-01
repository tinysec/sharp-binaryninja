using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeMemoryRegions(BNMemoryRegionInfo* regions, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeMemoryRegions"
        )]
		internal static extern void BNFreeMemoryRegions(
			
			// BNMemoryRegionInfo* regions
		    IntPtr regions   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
