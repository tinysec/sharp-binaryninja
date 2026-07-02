using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// int32_t BNLlvmServicesDisasmInstruction(const char* triplet, uint8_t* src, int32_t srcLen, uint64_t addr, const char* result, uint64_t resultMaxSize)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLlvmServicesDisasmInstruction"
        )]
		internal static extern int BNLlvmServicesDisasmInstruction(
			
			// const char* triplet
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string triplet  , 
			
			// uint8_t* src
		    IntPtr src  , 
			
			// int32_t srcLen
		    int srcLen  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// const char* result
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string result  , 
			
			// uint64_t resultMaxSize
		    ulong resultMaxSize  
			
		);
	}
}