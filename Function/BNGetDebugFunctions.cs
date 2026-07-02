using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDebugFunctionInfo* BNGetDebugFunctions(BNDebugInfo* debugInfo, const char* name, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDebugFunctions"
        )]
		internal static extern IntPtr BNGetDebugFunctions(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}