using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetStringAtAddress(BNBinaryView* view, uint64_t addr, BNStringReference* strRef)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetStringAtAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetStringAtAddress(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// BNStringReference* strRef
		    out BNStringReference strRef  
		);
	}
}