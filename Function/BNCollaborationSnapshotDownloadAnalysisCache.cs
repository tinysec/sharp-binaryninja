using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationSnapshotDownloadAnalysisCache(BNCollaborationSnapshot* snapshot, void** progress, void* progressContext, uint8_t** data, uint64_t* size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationSnapshotDownloadAnalysisCache"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationSnapshotDownloadAnalysisCache(
			
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