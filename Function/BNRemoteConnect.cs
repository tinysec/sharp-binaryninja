using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteConnect(BNRemote* remote, const char* username, const char* token)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteConnect"
        )]
		internal static extern bool BNRemoteConnect(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  , 
			
			// const char* token
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string token  
			
		);
	}
}