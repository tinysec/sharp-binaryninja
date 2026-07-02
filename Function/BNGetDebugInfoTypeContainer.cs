using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeContainer* BNGetDebugInfoTypeContainer(BNDebugInfo* debugInfo, const char* parserName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDebugInfoTypeContainer"
        )]
		internal static extern IntPtr BNGetDebugInfoTypeContainer(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* parserName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string parserName  
			
		);
	}
}