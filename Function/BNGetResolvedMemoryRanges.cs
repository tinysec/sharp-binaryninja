using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNResolvedMemoryRange* BNGetResolvedMemoryRanges(BNBinaryView* view, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetResolvedMemoryRanges"
        )]
		internal static extern IntPtr BNGetResolvedMemoryRanges(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
