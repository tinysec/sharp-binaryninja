using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetMemoryRegionInfo(BNBinaryView* view, const char* name, BNMemoryRegionInfo* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetMemoryRegionInfo"
        )]
		internal static extern bool BNGetMemoryRegionInfo(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// const char* name
		    string name   , 
			
			// BNMemoryRegionInfo* result
		    IntPtr result  
		);
	}
}
