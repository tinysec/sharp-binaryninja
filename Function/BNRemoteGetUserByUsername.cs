using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationUser* BNRemoteGetUserByUsername(BNRemote* remote, const char* username)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteGetUserByUsername"
        )]
		internal static extern IntPtr BNRemoteGetUserByUsername(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  
			
		);
	}
}