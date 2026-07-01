using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNAnalysisContextIsOffsetCodeSemantics(BNAnalysisContext* analysisContext, uint64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextIsOffsetCodeSemantics"
        )]
		internal static extern bool BNAnalysisContextIsOffsetCodeSemantics(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// uint64_t offset
		    ulong offset  
		);
	}
}
