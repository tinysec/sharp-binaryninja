using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNSection* BNAnalysisContextGetSectionByName(BNAnalysisContext* analysisContext, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisContextGetSectionByName"
        )]
		internal static extern IntPtr BNAnalysisContextGetSectionByName(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// const char* name
		    string name  
		);
	}
}
