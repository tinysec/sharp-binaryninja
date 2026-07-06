using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteSearchGroups(BNRemote* remote, const char* prefix, uint64_t** groupIds, const char*** groupNames, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteSearchGroups"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteSearchGroups(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char* prefix
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string prefix  , 
			
			// uint64_t** groupIds
		    IntPtr groupIds  , 
			
			// const char*** groupNames
		    IntPtr groupNames  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}