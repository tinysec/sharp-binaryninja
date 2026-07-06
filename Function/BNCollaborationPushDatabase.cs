using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationPushDatabase(BNDatabase* database, BNRemoteFile* file, uint64_t* count, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationPushDatabase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationPushDatabase(
			
			// BNDatabase* database
		    IntPtr database  , 
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// uint64_t* count
		    IntPtr count  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
		);
	}
}