using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNType* BNCreateWideCharType(uint64_t width, const char* altName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateWideCharType"
        )]
		internal static extern IntPtr BNCreateWideCharType(
			
			// uint64_t width
		    ulong width  , 
			
			// const char* altName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string altName  
		);
	}
}