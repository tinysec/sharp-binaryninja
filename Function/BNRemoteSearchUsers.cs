using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteSearchUsers(BNRemote* remote, const char* prefix, const char*** userIds, const char*** usernames, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteSearchUsers"
        )]
		internal static extern bool BNRemoteSearchUsers(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* prefix
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prefix  , 
			
			// const char*** userIds
		    IntPtr userIds  , 
			
			// const char*** usernames
		    IntPtr usernames  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}