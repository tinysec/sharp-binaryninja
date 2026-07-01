using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreePluginVersions(BNPluginVersion* r, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreePluginVersions"
        )]
		internal static extern void BNFreePluginVersions(
			
			// BNPluginVersion* r
		    IntPtr r   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
