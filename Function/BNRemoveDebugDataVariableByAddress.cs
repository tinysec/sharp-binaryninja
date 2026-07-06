using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoveDebugDataVariableByAddress(BNDebugInfo* debugInfo, const char* parserName, uint64_t address)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoveDebugDataVariableByAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRemoveDebugDataVariableByAddress(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* parserName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string parserName  , 
			
			// uint64_t address
		    ulong address  
			
		);
	}
}