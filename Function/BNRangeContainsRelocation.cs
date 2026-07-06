using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRangeContainsRelocation(BNBinaryView* view, uint64_t addr, uint64_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRangeContainsRelocation"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRangeContainsRelocation(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t size
		    ulong size  
		);
	}
}