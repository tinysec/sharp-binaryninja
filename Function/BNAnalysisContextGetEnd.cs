using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNAnalysisContextGetEnd(BNAnalysisContext* analysisContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextGetEnd"
        )]
		internal static extern ulong BNAnalysisContextGetEnd(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext  
		);
	}
}
