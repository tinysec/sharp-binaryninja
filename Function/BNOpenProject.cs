using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNProject* BNOpenProject(const char* path)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNOpenProject"
        )]
		internal static extern IntPtr BNOpenProject(
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  
			
		);
	}
}