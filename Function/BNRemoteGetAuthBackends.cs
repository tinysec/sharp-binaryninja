using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteGetAuthBackends(BNRemote* remote, const char*** backendIds, const char*** backendNames, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteGetAuthBackends"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteGetAuthBackends(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// const char*** backendIds
		    IntPtr backendIds  , 
			
			// const char*** backendNames
		    IntPtr backendNames  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}