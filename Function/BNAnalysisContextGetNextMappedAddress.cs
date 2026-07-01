using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNAnalysisContextGetNextMappedAddress(BNAnalysisContext* analysisContext, uint64_t addr, uint32_t flags)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextGetNextMappedAddress"
        )]
		internal static extern ulong BNAnalysisContextGetNextMappedAddress(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// uint64_t addr
		    ulong addr   , 
			
			// uint32_t flags
		    uint flags  
		);
	}
}
