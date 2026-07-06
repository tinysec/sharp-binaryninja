using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteFileDeleteSnapshot(BNRemoteFile* file, BNCollaborationSnapshot* snapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteFileDeleteSnapshot"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteFileDeleteSnapshot(
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// BNCollaborationSnapshot* snapshot
		    IntPtr snapshot  
			
		);
	}
}