using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteDeleteGroup(BNRemote* remote, BNCollaborationGroup* group)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteDeleteGroup"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteDeleteGroup(
			
			// BNRemote* remote
		    IntPtr remote  , 
			
			// BNCollaborationGroup* _group
		    IntPtr _group  
			
		);
	}
}