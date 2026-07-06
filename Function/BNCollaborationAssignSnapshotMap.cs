using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationAssignSnapshotMap(BNSnapshot* localSnapshot, BNCollaborationSnapshot* remoteSnapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationAssignSnapshotMap"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationAssignSnapshotMap(
			
			// BNSnapshot* localSnapshot
		    IntPtr localSnapshot  , 
			
			// BNCollaborationSnapshot* remoteSnapshot
		    IntPtr remoteSnapshot  
		);
	}
}