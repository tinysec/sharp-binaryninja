using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationSnapshotDownloadSnapshotFile(BNCollaborationSnapshot* snapshot, void** progress, void* progressContext, uint8_t** data, uint64_t* size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationSnapshotDownloadSnapshotFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationSnapshotDownloadSnapshotFile(
			
			// BNCollaborationSnapshot* snapshot
		    IntPtr snapshot  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  , 
			
			// uint8_t** data
		    IntPtr data  , 
			
			// uint64_t* size
		    IntPtr size  
		);
	}
}