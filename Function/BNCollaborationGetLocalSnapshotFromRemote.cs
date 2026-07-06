using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationGetLocalSnapshotFromRemote(BNCollaborationSnapshot* snapshot, BNDatabase* database, BNSnapshot** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
     
            EntryPoint = "BNCollaborationGetLocalSnapshotFromRemote"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationGetLocalSnapshotFromRemote(
			
			// BNCollaborationSnapshot* snapshot
		    IntPtr snapshot  , 
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNSnapshot** result
		    IntPtr result  
		);
	}
}