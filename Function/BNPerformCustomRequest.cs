using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// int32_t BNPerformCustomRequest(BNDownloadInstance* instance, const char* method, const char* url, uint64_t headerCount, const char** headerKeys, const char** headerValues, BNDownloadInstanceResponse** response, BNDownloadInstanceInputOutputCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPerformCustomRequest"
        )]
		internal static extern int BNPerformCustomRequest(
			
			// BNDownloadInstance* instance
		    IntPtr instance  , 
			
			// const char* method
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string method  , 
			
			// const char* url
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string url  , 
			
			// uint64_t headerCount
		    ulong headerCount  , 
			
			// const char** headerKeys
		    string[] headerKeys  , 
			
			// const char** headerValues
		    string[] headerValues  , 
			
			// BNDownloadInstanceResponse** response
		    IntPtr response  , 
			
			// BNDownloadInstanceInputOutputCallbacks* callbacks
		    IntPtr callbacks  
			
		);
	}
}