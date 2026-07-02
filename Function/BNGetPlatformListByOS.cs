using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNPlatform** BNGetPlatformListByOS(const char* os, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetPlatformListByOS"
        )]
		internal static extern IntPtr BNGetPlatformListByOS(
			
			// const char* os
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string os  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}