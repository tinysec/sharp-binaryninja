using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFileSize(const char* path, uint64_t* size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFileSize"
        )]
		internal static extern bool BNFileSize(
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  , 
			
			// uint64_t* size
		    IntPtr size  
			
		);
	}
}