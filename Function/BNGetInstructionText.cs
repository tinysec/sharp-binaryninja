using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetInstructionText(BNArchitecture* arch, uint8_t* data, uint64_t addr, uint64_t* len, BNInstructionTextToken** result, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetInstructionText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetInstructionText(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint8_t* data
		    byte[] data  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// uint64_t* len
		    ref ulong len  , 
			
			// BNInstructionTextToken** result
		    out IntPtr result  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}