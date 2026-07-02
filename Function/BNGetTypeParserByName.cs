using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeParser* BNGetTypeParserByName(const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeParserByName"
        )]
		internal static extern IntPtr BNGetTypeParserByName(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
			
		);
	}
}