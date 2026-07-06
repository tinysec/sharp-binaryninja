using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationMergeDatabase(BNDatabase* database, void** conflictHandler, void* conflictHandlerCtxt, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationMergeDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationMergeDatabase(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// void** conflictHandler
		    IntPtr conflictHandler  , 
			
			// void* conflictHandlerCtxt
		    IntPtr conflictHandlerCtxt  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
		);
	}
}