using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsArchitectureInvertBranchPatchAvailable(BNArchitecture* arch, uint8_t* data, uint64_t addr, uint64_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsArchitectureInvertBranchPatchAvailable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsArchitectureInvertBranchPatchAvailable(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint8_t* data
		    byte[] data  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t len
		    ulong len  
		);
	}
}