using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeResolvedMemoryRange(BNResolvedMemoryRange* range)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeResolvedMemoryRange"
        )]
		internal static extern void BNFreeResolvedMemoryRange(
			
			// BNResolvedMemoryRange* range
		    IntPtr range  
		);
	}
}
