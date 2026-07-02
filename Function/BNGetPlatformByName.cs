using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNPlatform* BNGetPlatformByName(const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetPlatformByName"
        )]
		internal static extern IntPtr BNGetPlatformByName(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
		);
	}
}