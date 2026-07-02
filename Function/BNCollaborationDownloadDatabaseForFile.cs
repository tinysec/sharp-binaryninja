using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCollaborationDownloadDatabaseForFile(BNRemoteFile* file, const char* dbPath, bool force, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationDownloadDatabaseForFile"
        )]
		internal static extern bool BNCollaborationDownloadDatabaseForFile(
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// const char* dbPath
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string dbPath  , 
			
			// bool force
		    bool force  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
		);
	}
}