using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDownloadProvider* BNRegisterDownloadProvider(const char* name, BNDownloadProviderCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterDownloadProvider"
        )]
		internal static extern IntPtr BNRegisterDownloadProvider(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNDownloadProviderCallbacks* callbacks
		    IntPtr callbacks  
			
		);
	}
}