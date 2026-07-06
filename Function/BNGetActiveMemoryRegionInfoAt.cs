using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetActiveMemoryRegionInfoAt(BNBinaryView* view, uint64_t addr, BNMemoryRegionInfo* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetActiveMemoryRegionInfoAt"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetActiveMemoryRegionInfoAt(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// uint64_t addr
		    ulong addr   , 
			
			// BNMemoryRegionInfo* result
		    IntPtr result  
		);
	}
}
