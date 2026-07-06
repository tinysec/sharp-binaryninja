using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNPluginIsDisablePending(BNRepoPlugin* p)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPluginIsDisablePending"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNPluginIsDisablePending(
			
			// BNRepoPlugin* p
		    IntPtr p  
			
		);
	}
}