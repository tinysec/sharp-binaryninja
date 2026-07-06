using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationSnapshotFinalize(BNCollaborationSnapshot* snapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationSnapshotFinalize"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationSnapshotFinalize(
			
			// BNCollaborationSnapshot* snapshot
		    IntPtr snapshot  
		);
	}
}