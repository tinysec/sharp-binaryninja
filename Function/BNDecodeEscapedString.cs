using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDataBuffer* BNDecodeEscapedString(const char* str)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDecodeEscapedString"
        )]
		internal static extern IntPtr BNDecodeEscapedString(
			
			// const char* str
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string str  
			
		);
	}
}