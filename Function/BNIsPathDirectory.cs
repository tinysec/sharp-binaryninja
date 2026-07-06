using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsPathDirectory(const char* path)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsPathDirectory"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsPathDirectory(
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  
			
		);
	}
}