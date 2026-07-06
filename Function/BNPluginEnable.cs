using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNPluginEnable(BNRepoPlugin* p, bool force)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPluginEnable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNPluginEnable(
			
			// BNRepoPlugin* p
		    IntPtr p  , 
			
			// bool force
		    bool force  
			
		);
	}
}