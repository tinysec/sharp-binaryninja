using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoteProjectOpen(BNRemoteProject* project, void** progress, void* progressCtxt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoteProjectOpen"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoteProjectOpen(
			
			// BNRemoteProject* project
		    IntPtr project  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* progressCtxt
		    IntPtr progressCtxt  
			
		);
	}
}