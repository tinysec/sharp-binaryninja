using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNNameAndType* BNGetDebugTypesByName(BNDebugInfo* debugInfo, const char* typeName, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetDebugTypesByName"
        )]
		internal static extern IntPtr BNGetDebugTypesByName(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// const char* typeName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeName  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}