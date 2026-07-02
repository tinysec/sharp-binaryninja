using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNFileMetadata* BNCollaborationDownloadFile(BNRemoteFile* file, const char* dbPath, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationDownloadFile"
        )]
		internal static extern IntPtr BNCollaborationDownloadFile(
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// const char* dbPath
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string dbPath  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
		);
	}
}