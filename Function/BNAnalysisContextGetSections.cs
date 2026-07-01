using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNSection** BNAnalysisContextGetSections(BNAnalysisContext* analysisContext, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextGetSections"
        )]
		internal static extern IntPtr BNAnalysisContextGetSections(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
