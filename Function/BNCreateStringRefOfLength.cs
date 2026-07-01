using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNStringRef* BNCreateStringRefOfLength(const char* str, size_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateStringRefOfLength"
        )]
		internal static extern IntPtr BNCreateStringRefOfLength(
			
			// const char* str
		    string str   , 
			
			// size_t len
		    UIntPtr len  
		);
	}
}
