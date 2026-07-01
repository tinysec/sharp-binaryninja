using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNPluginGetCurrentVersionCreationDate(BNPlugin* p)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPluginGetCurrentVersionCreationDate"
        )]
		internal static extern IntPtr BNPluginGetCurrentVersionCreationDate(
			
			// BNPlugin* p
		    IntPtr p  
		);
	}
}
