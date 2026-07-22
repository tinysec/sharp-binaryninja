using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNGetInstructionTextWithContext(BNArchitecture* arch, const uint8_t* data, uint64_t addr, size_t* len, void* context, BNInstructionTextToken** result, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetInstructionTextWithContext"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetInstructionTextWithContext(
			
			// BNArchitecture* arch
		    IntPtr arch   , 
			
			// const uint8_t* data
		    byte[] data,
			
			// uint64_t addr
		    ulong addr   , 
			
			// size_t* len
		    ref UIntPtr len,
			
			// void* context
		    IntPtr context   , 
			
			// BNInstructionTextToken** result
		    out IntPtr result,
			
			// size_t* count
		    out UIntPtr count
		);
	}
}
