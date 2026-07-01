using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNPluginFreeVersion(BNPluginVersion v)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNPluginFreeVersion"
        )]
		internal static extern void BNPluginFreeVersion(
			
			// BNPluginVersion v
		    BNPluginVersion v  
		);
	}
}
