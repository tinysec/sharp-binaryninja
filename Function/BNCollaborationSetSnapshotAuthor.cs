using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationSetSnapshotAuthor(BNDatabase* database, BNSnapshot* snapshot, const char* author)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationSetSnapshotAuthor"
        )]
		internal static extern bool BNCollaborationSetSnapshotAuthor(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNSnapshot* snapshot
		    IntPtr snapshot  , 
			
			// const char* author
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string author  
		);
	}
}