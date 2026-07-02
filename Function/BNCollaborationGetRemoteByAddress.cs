using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRemote* BNCollaborationGetRemoteByAddress(const char* remoteAddress)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationGetRemoteByAddress"
        )]
		internal static extern IntPtr BNCollaborationGetRemoteByAddress(
			
			// const char* remoteAddress
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string remoteAddress  
		);
	}
}