using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDataVariableAndName* BNGetDebugDataVariablesByName(BNDebugInfo* debugInfo, const char* variableName, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDebugDataVariablesByName"
        )]
		internal static extern IntPtr BNGetDebugDataVariablesByName(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* variableName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string variableName  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}