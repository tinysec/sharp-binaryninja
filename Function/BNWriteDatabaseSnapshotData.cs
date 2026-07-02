using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// int64_t BNWriteDatabaseSnapshotData(BNDatabase* database, int64_t* parents, uint64_t parentCount, BNBinaryView* file, const char* name, BNKeyValueStore* data, bool autoSave, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWriteDatabaseSnapshotData"
        )]
		internal static extern long BNWriteDatabaseSnapshotData(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// int64_t* parents
		    IntPtr parents  , 
			
			// uint64_t parentCount
		    ulong parentCount  , 
			
			// BNBinaryView* file
		    IntPtr file  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNKeyValueStore* data
		    IntPtr data  , 
			
			// bool autoSave
		    bool autoSave  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  
			
		);
	}
}