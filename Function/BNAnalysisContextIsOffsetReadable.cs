using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNAnalysisContextIsOffsetReadable(BNAnalysisContext* analysisContext, uint64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAnalysisContextIsOffsetReadable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAnalysisContextIsOffsetReadable(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// uint64_t offset
		    ulong offset  
		);
	}
}
