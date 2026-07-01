using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeMemoryRegionInfo(BNMemoryRegionInfo* info)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeMemoryRegionInfo"
        )]
		internal static extern void BNFreeMemoryRegionInfo(
			
			// BNMemoryRegionInfo* info
		    IntPtr info  
		);
	}
}
