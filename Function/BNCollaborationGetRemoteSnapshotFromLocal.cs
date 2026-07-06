using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationGetRemoteSnapshotFromLocal(BNSnapshot* snapshot, BNCollaborationSnapshot** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationGetRemoteSnapshotFromLocal"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationGetRemoteSnapshotFromLocal(
			
			// BNSnapshot* snapshot
		    IntPtr snapshot  , 
			
			// BNCollaborationSnapshot** result
		    IntPtr result  
		);
	}
}