using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteProjectDeletePermission(BNRemoteProject* project, BNCollaborationPermission* permission)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectDeletePermission"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteProjectDeletePermission(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// BNCollaborationPermission* permission
		    IntPtr permission  
			
		);
	}
}