using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetInstructionContainingAddress(BNFunction* func, BNArchitecture* arch, uint64_t addr, uint64_t* start)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetInstructionContainingAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetInstructionContainingAddress(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t* start
		    out ulong start  
		);
	}
}