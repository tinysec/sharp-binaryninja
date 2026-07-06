using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationUploadTypeArchive(BNTypeArchive* archive, BNRemoteProject* project, BNRemoteFolder* folder, void** progress, void* progressContext, BNProjectFile* coreFile, BNRemoteFile** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCollaborationUploadTypeArchive"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCollaborationUploadTypeArchive(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// BNRemoteFolder* folder
		    IntPtr folder  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  , 
			
			// BNProjectFile* coreFile
		    IntPtr coreFile  , 
			
			// BNRemoteFile** result
		    IntPtr result  
		);
	}
}