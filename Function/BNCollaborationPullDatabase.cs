using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationPullDatabase(BNDatabase* database, BNRemoteFile* file, uint64_t* count, void** conflictHandler, void* conflictHandlerCtxt, void** progress, void* progressContext, void** nameChangeset, void* nameChangesetContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationPullDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationPullDatabase(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// uint64_t* count
		    IntPtr count  , 
			
			// void** conflictHandler
		    IntPtr conflictHandler  , 
			
			// void* conflictHandlerCtxt
		    IntPtr conflictHandlerCtxt  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  , 
			
			// void** nameChangeset
		    IntPtr nameChangeset  , 
			
			// void* nameChangesetContext
		    IntPtr nameChangesetContext  
		);
	}
}