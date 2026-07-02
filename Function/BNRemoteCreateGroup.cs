using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCollaborationGroup* BNRemoteCreateGroup(BNRemote* remote, const char* name, const char** usernames, uint64_t usernameCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteCreateGroup"
        )]
		internal static extern IntPtr BNRemoteCreateGroup(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char** usernames
		    string[] usernames  , 
			
			// uint64_t usernameCount
		    ulong usernameCount  
			
		);
	}
}