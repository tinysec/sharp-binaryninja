using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNGetMemoryRegionDisplayName(BNBinaryView* view, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetMemoryRegionDisplayName"
        )]
		internal static extern IntPtr BNGetMemoryRegionDisplayName(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// const char* name
		    string name  
		);
	}
}
