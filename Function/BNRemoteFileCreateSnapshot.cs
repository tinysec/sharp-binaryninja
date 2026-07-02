using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationSnapshot* BNRemoteFileCreateSnapshot(BNRemoteFile* file, const char* name, uint8_t* contents, uint64_t contentsSize, uint8_t* analysisCacheContents, uint64_t analysisCacheContentsSize, uint8_t* fileContents, uint64_t fileContentsSize, const char** parentIds, uint64_t parentIdCount, void** progress, void* progressContext)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteFileCreateSnapshot"
        )]
		internal static extern IntPtr BNRemoteFileCreateSnapshot(
			
			// BNRemoteFile* file
		    IntPtr file  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint8_t* contents
		    IntPtr contents  , 
			
			// uint64_t contentsSize
		    ulong contentsSize  , 
			
			// uint8_t* analysisCacheContents
		    IntPtr analysisCacheContents  , 
			
			// uint64_t analysisCacheContentsSize
		    ulong analysisCacheContentsSize  , 
			
			// uint8_t* fileContents
		    IntPtr fileContents  , 
			
			// uint64_t fileContentsSize
		    ulong fileContentsSize  , 
			
			// const char** parentIds
		    string[] parentIds  , 
			
			// uint64_t parentIdCount
		    ulong parentIdCount  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressContext
		    IntPtr progressContext  
			
		);
	}
}