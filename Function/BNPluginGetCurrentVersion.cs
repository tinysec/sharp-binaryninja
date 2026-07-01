using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNPluginVersion BNPluginGetCurrentVersion(BNPlugin* p)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPluginGetCurrentVersion"
        )]
		internal static extern BNPluginVersion BNPluginGetCurrentVersion(
			
			// BNPlugin* p
		    IntPtr p  
		);
	}
}
