using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationSyncDatabase(BNDatabase* database, BNRemoteFile* file, void** conflictHandler, void* conflictHandlerCtxt, void** progress, void* progressCtxt, void** nameChangeset, void* nameChangesetCtxt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationSyncDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationSyncDatabase(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// void** conflictHandler
		    IntPtr conflictHandler  , 
			
			// void* conflictHandlerCtxt
		    IntPtr conflictHandlerCtxt  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressCtxt
		    IntPtr progressCtxt  , 
			
			// void** nameChangeset
		    IntPtr nameChangeset  , 
			
			// void* nameChangesetCtxt
		    IntPtr nameChangesetCtxt  
		);
	}
}