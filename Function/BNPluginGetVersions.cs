using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNPluginVersion* BNPluginGetVersions(BNPlugin* p, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPluginGetVersions"
        )]
		internal static extern IntPtr BNPluginGetVersions(
			
			// BNPlugin* p
		    IntPtr p   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
