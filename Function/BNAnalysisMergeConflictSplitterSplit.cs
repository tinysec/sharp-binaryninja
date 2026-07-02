using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAnalysisMergeConflictSplitterSplit(BNAnalysisMergeConflictSplitter* splitter, const char* originalKey, BNAnalysisMergeConflict* originalConflict, BNKeyValueStore* result, const char*** newKeys, BNAnalysisMergeConflict*** newConflicts, uint64_t* newCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisMergeConflictSplitterSplit"
        )]
		internal static extern bool BNAnalysisMergeConflictSplitterSplit(
			
			// BNAnalysisMergeConflictSplitter* splitter
		    IntPtr splitter  , 
			
			// const char* originalKey
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string originalKey  , 
			
			// BNAnalysisMergeConflict* originalConflict
		    IntPtr originalConflict  , 
			
			// BNKeyValueStore* result
		    IntPtr result  , 
			
			// const char*** newKeys
		    IntPtr newKeys  , 
			
			// BNAnalysisMergeConflict*** newConflicts
		    IntPtr newConflicts  , 
			
			// uint64_t* newCount
		    IntPtr newCount  
		);
	}
}