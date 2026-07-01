using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNPluginCommand* BNGetValidPluginCommandsGlobal(size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetValidPluginCommandsGlobal"
        )]
		internal static extern IntPtr BNGetValidPluginCommandsGlobal(
			
			// size_t* count
		    IntPtr count  
		);
	}
}
