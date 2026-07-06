using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationIsSnapshotIgnored(BNDatabase* database, BNSnapshot* snapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationIsSnapshotIgnored"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationIsSnapshotIgnored(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNSnapshot* snapshot
		    IntPtr snapshot  
		);
	}
}