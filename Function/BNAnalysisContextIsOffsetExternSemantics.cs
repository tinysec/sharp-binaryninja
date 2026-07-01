using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNAnalysisContextIsOffsetExternSemantics(BNAnalysisContext* analysisContext, uint64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextIsOffsetExternSemantics"
        )]
		internal static extern bool BNAnalysisContextIsOffsetExternSemantics(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// uint64_t offset
		    ulong offset  
		);
	}
}
