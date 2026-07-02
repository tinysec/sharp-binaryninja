using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetDebugDataVariableByAddress(BNDebugInfo* debugInfo, const char* parserName, uint64_t address, BNDataVariableAndName* var)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDebugDataVariableByAddress"
        )]
		internal static extern bool BNGetDebugDataVariableByAddress(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* parserName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string parserName  , 
			
			// uint64_t address
		    ulong address  , 
			
			// BNDataVariableAndName* _var
		    IntPtr _var  
			
		);
	}
}