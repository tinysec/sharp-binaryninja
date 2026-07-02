using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAnalysisMergeConflictSuccess(BNAnalysisMergeConflict* conflict, const char* value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisMergeConflictSuccess"
        )]
		internal static extern bool BNAnalysisMergeConflictSuccess(
			
			// BNAnalysisMergeConflict* conflict
		    IntPtr conflict  , 
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  
		);
	}
}