using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// const bool BNPluginGetIsPaid(BNPlugin* p)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPluginGetIsPaid"
        )]
		internal static extern bool BNPluginGetIsPaid(
			
			// BNPlugin* p
		    IntPtr p  
		);
	}
}
