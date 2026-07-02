using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDataVariableAndName* BNGetDebugDataVariables(BNDebugInfo* debugInfo, const char* name, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDebugDataVariables"
        )]
		internal static extern IntPtr BNGetDebugDataVariables(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}