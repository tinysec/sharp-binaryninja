using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNAnalysisMergeConflictGetPathItemSerialized(BNAnalysisMergeConflict* conflict, const char* path)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisMergeConflictGetPathItemSerialized"
        )]
		internal static extern IntPtr BNAnalysisMergeConflictGetPathItemSerialized(
			
			// BNAnalysisMergeConflict* conflict
		    IntPtr conflict  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  
		);
	}
}