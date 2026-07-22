using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeArchiveMergeSnapshots(BNTypeArchive* archive, const char* baseSnapshot, const char* firstSnapshot, const char* secondSnapshot, const char** mergeConflictKeysIn, const char** mergeConflictValuesIn, uint64_t mergeConflictCountIn, const char*** mergeConflictsOut, uint64_t* mergeConflictCountOut, const char** result, void** progress, void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeArchiveMergeSnapshots"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeArchiveMergeSnapshots(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* baseSnapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string baseSnapshot  , 
			
			// const char* firstSnapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string firstSnapshot  , 
			
			// const char* secondSnapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string secondSnapshot  , 
			
			// const char** mergeConflictKeysIn
		    IntPtr mergeConflictKeysIn  ,
			
			// const char** mergeConflictValuesIn
		    IntPtr mergeConflictValuesIn  ,
			
			// uint64_t mergeConflictCountIn
		    ulong mergeConflictCountIn  , 
			
			// const char*** mergeConflictsOut
		    out IntPtr mergeConflictsOut  ,
			
			// uint64_t* mergeConflictCountOut
		    out ulong mergeConflictCountOut  ,
			
			// const char** result
		    out IntPtr result  ,
			
			// void** progress
		    IntPtr progress  , 
			
			// void* context
		    IntPtr context  
			
		);
	}
}
