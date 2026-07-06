using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSkipAndReturnValue(BNBinaryView* view, BNArchitecture* arch, uint64_t addr, uint64_t value)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSkipAndReturnValue"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSkipAndReturnValue(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t _value
		    ulong value  
		);
	}
}