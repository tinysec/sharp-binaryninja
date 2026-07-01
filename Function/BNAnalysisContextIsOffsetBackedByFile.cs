using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNAnalysisContextIsOffsetBackedByFile(BNAnalysisContext* analysisContext, uint64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextIsOffsetBackedByFile"
        )]
		internal static extern bool BNAnalysisContextIsOffsetBackedByFile(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// uint64_t offset
		    ulong offset  
		);
	}
}
