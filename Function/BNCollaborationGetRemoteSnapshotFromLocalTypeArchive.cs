using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationSnapshot* BNCollaborationGetRemoteSnapshotFromLocalTypeArchive(BNTypeArchive* archive, const char* snapshotId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationGetRemoteSnapshotFromLocalTypeArchive"
        )]
		internal static extern IntPtr BNCollaborationGetRemoteSnapshotFromLocalTypeArchive(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* snapshotId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string snapshotId  
		);
	}
}