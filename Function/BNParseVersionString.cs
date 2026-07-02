using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNVersionInfo BNParseVersionString(const char* v)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParseVersionString"
        )]
		internal static extern BNVersionInfo BNParseVersionString(
			
			// const char* v
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string v  
		);
	}
}
