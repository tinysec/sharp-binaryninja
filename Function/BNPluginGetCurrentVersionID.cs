using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// const char* BNPluginGetCurrentVersionID(BNPlugin* p)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPluginGetCurrentVersionID"
        )]
		internal static extern IntPtr BNPluginGetCurrentVersionID(
			
			// BNPlugin* p
		    IntPtr p  
		);
	}
}
