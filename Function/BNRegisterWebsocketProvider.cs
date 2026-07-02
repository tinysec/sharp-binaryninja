using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNWebsocketProvider* BNRegisterWebsocketProvider(const char* name, BNWebsocketProviderCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterWebsocketProvider"
        )]
		internal static extern IntPtr BNRegisterWebsocketProvider(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNWebsocketProviderCallbacks* callbacks
		    IntPtr callbacks  
			
		);
	}
}