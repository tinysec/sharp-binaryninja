using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNSection** BNAnalysisContextGetSectionsAt(BNAnalysisContext* analysisContext, uint64_t addr, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextGetSectionsAt"
        )]
		internal static extern IntPtr BNAnalysisContextGetSectionsAt(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// uint64_t addr
		    ulong addr   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
