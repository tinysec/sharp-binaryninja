using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRemoteProject* BNRemoteGetProjectById(BNRemote* remote, const char* id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteGetProjectById"
        )]
		internal static extern IntPtr BNRemoteGetProjectById(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  
			
		);
	}
}