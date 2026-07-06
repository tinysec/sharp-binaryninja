using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationGetSnapshotAuthor(BNDatabase* database, BNSnapshot* snapshot, const char** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationGetSnapshotAuthor"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationGetSnapshotAuthor(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNSnapshot* snapshot
		    IntPtr snapshot  , 
			
			// const char** result
		    string[] result  
		);
	}
}