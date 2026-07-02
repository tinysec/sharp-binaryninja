using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// int32_t BNPerformDownloadRequest(BNDownloadInstance* instance, const char* url, BNDownloadInstanceOutputCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPerformDownloadRequest"
        )]
		internal static extern int BNPerformDownloadRequest(
			
			// BNDownloadInstance* instance
		    IntPtr instance  , 
			
			// const char* url
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string url  , 
			
			// BNDownloadInstanceOutputCallbacks* callbacks
		    IntPtr callbacks  
			
		);
	}
}