using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNAddressRange* BNAnalysisContextGetBackedAddressRanges(BNAnalysisContext* analysisContext, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextGetBackedAddressRanges"
        )]
		internal static extern IntPtr BNAnalysisContextGetBackedAddressRanges(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
