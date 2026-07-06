using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAnalysisMergeConflictSplitterCanSplit(BNAnalysisMergeConflictSplitter* splitter, const char* key, BNAnalysisMergeConflict* conflict)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisMergeConflictSplitterCanSplit"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAnalysisMergeConflictSplitterCanSplit(
			
			// BNAnalysisMergeConflictSplitter* splitter
		    IntPtr splitter  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNAnalysisMergeConflict* conflict
		    IntPtr conflict  
		);
	}
}