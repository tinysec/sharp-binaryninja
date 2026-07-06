using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetResolvedMemoryRangeAt(BNBinaryView* view, uint64_t addr, BNResolvedMemoryRange* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetResolvedMemoryRangeAt"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetResolvedMemoryRangeAt(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// uint64_t addr
		    ulong addr   , 
			
			// BNResolvedMemoryRange* result
		    IntPtr result  
		);
	}
}
