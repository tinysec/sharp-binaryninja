using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNAnalysisContextGetLength(BNAnalysisContext* analysisContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextGetLength"
        )]
		internal static extern ulong BNAnalysisContextGetLength(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext  
		);
	}
}
