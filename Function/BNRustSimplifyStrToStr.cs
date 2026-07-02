using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNRustSimplifyStrToStr(const char* param1)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRustSimplifyStrToStr"
        )]
		internal static extern IntPtr BNRustSimplifyStrToStr(
			
			// const char* param1
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string param1  
			
		);
	}
}