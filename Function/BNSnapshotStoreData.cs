using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSnapshotStoreData(BNSnapshot* snapshot, BNKeyValueStore* data, void* ctxt, void** progress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSnapshotStoreData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSnapshotStoreData(
			
			// BNSnapshot* snapshot
		    IntPtr snapshot  , 
			
			// BNKeyValueStore* data
		    IntPtr data  , 
			
			// void* ctxt
		    IntPtr ctxt  , 
			
			// void** progress
		    IntPtr progress  
			
		);
	}
}