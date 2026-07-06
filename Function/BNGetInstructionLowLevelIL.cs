using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetInstructionLowLevelIL(BNArchitecture* arch, uint8_t* data, uint64_t addr, uint64_t* len, BNLowLevelILFunction* il)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetInstructionLowLevelIL"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetInstructionLowLevelIL(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const uint8_t* data
		    byte[] data  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t* len
		    ref ulong len  , 
			
			// BNLowLevelILFunction* il
		    IntPtr il  
		);
	}
}