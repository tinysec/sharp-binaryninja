using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNAllocString(const char* contents)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAllocString"
        )]
		internal static extern IntPtr BNAllocString(
			
			// const char* contents
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string contents  
		);
	}
}