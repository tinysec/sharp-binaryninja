using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationSnapshot* BNRemoteFileGetSnapshotById(BNRemoteFile* file, const char* id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteFileGetSnapshotById"
        )]
		internal static extern IntPtr BNRemoteFileGetSnapshotById(
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  
			
		);
	}
}