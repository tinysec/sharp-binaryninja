using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetInstructionInfo(BNArchitecture* arch, uint8_t* data, uint64_t addr, uint64_t maxLen, BNInstructionInfo* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetInstructionInfo"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetInstructionInfo(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint8_t* data
		    byte[] data  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t maxLen
		    ulong maxLen  , 
			
			// BNInstructionInfo* result
			out BNInstructionInfo result  
		);
	}
}