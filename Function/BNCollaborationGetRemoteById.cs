using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNRemote* BNCollaborationGetRemoteById(const char* remoteId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationGetRemoteById"
        )]
		internal static extern IntPtr BNCollaborationGetRemoteById(
			
			// const char* remoteId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string remoteId  
		);
	}
}