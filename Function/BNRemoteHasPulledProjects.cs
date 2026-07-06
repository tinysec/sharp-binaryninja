using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteHasPulledProjects(BNRemote* remote)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteHasPulledProjects"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteHasPulledProjects(
			
			// BNRemote* remote
		    IntPtr remote  
			
		);
	}
}