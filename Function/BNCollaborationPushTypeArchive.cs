using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationPushTypeArchive(BNTypeArchive* archive, BNRemoteFile* file, uint64_t* count, void** progress, void* progressCtxt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationPushTypeArchive"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationPushTypeArchive(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// uint64_t* count
		    IntPtr count  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressCtxt
		    IntPtr progressCtxt  
		);
	}
}