using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNUnregisterDebugInfoParser(const char* rawName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUnregisterDebugInfoParser"
        )]
		internal static extern void BNUnregisterDebugInfoParser(
			
			// const char* rawName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string rawName  
			
		);
	}
}