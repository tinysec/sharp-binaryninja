using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNAllocStringWithLength(const char* contents, uint64_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAllocStringWithLength"
        )]
		internal static extern IntPtr BNAllocStringWithLength(
			
			// const char* contents
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string contents  , 
			
			// uint64_t len
		    ulong len  
		);
	}
}