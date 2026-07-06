using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationSyncTypeArchive(BNTypeArchive* archive, BNRemoteFile* file, void** conflictHandler, void* conflictHandlerCtxt, void** progress, void* progressCtxt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationSyncTypeArchive"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationSyncTypeArchive(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// void** conflictHandler
		    IntPtr conflictHandler  , 
			
			// void* conflictHandlerCtxt
		    IntPtr conflictHandlerCtxt  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressCtxt
		    IntPtr progressCtxt  
		);
	}
}