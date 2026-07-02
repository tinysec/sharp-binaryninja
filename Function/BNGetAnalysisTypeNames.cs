using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNQualifiedName* BNGetAnalysisTypeNames(BNBinaryView* view, uint64_t* count, const char* matching)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetAnalysisTypeNames"
        )]
		internal static extern IntPtr BNGetAnalysisTypeNames(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t* count
		    out ulong count  , 
			
			// const char* matching
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string matching  
		);
	}
}