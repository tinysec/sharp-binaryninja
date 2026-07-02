using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNRemoteRequestAuthenticationToken(BNRemote* remote, const char* username, const char* password)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteRequestAuthenticationToken"
        )]
		internal static extern IntPtr BNRemoteRequestAuthenticationToken(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  , 
			
			// const char* password
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string password  
			
		);
	}
}