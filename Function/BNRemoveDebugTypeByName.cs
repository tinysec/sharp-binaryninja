using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRemoveDebugTypeByName(BNDebugInfo* debugInfo, const char* parserName, const char* typeName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRemoveDebugTypeByName"
        )]
		internal static extern bool BNRemoveDebugTypeByName(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* parserName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string parserName  , 
			
			// const char* typeName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeName  
			
		);
	}
}