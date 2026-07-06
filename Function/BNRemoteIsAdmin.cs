using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteIsAdmin(BNRemote* remote)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteIsAdmin"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteIsAdmin(
			
			// BNRemote* remote
		    IntPtr remote  
			
		);
	}
}