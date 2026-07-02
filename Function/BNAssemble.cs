using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAssemble(BNArchitecture* arch, const char* code, uint64_t addr, BNDataBuffer* result, const char** errors)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAssemble"
        )]
		internal static extern bool BNAssemble(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* code
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string code  , 
			
			// uint64_t addr
		    ulong addr  , 
			
			// BNDataBuffer* result
		    IntPtr result  , 
			
			// const char** errors
		    out IntPtr errors  
		);
	}
}