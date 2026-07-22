using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNMemoryRegionInfo* BNGetMemoryRegions(BNBinaryView* view, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetMemoryRegions"
        )]
		internal static extern IntPtr BNGetMemoryRegions(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// size_t* count
		    out UIntPtr count
		);
	}
}
