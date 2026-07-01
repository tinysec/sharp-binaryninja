using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeResolvedMemoryRanges(BNResolvedMemoryRange* ranges, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeResolvedMemoryRanges"
        )]
		internal static extern void BNFreeResolvedMemoryRanges(
			
			// BNResolvedMemoryRange* ranges
		    IntPtr ranges   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
