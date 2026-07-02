using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRemoteFile* BNRemoteProjectCreateFile(BNRemoteProject* project, const char* filename, uint8_t* contents, uint64_t contentsSize, const char* name, const char* description, BNRemoteFolder* folder, BNRemoteFileType type, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectCreateFile"
        )]
		internal static extern IntPtr BNRemoteProjectCreateFile(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename  , 
			
			// uint8_t* contents
		    IntPtr contents  , 
			
			// uint64_t contentsSize
		    ulong contentsSize  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* description
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string description  , 
			
			// BNRemoteFolder* folder
		    IntPtr folder  , 
			
			// BNRemoteFileType type
		    RemoteFileType type  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
			
		);
	}
}