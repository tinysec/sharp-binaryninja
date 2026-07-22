using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNAddressRange* BNAnalysisContextGetMappedAddressRanges(BNAnalysisContext* analysisContext, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextGetMappedAddressRanges"
        )]
		internal static extern IntPtr BNAnalysisContextGetMappedAddressRanges(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// size_t* count
		    out ulong count
		);
	}
}
