using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNPlatform** BNGetPlatformListByOSAndArchitecture(const char* os, BNArchitecture* arch, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetPlatformListByOSAndArchitecture"
        )]
		internal static extern IntPtr BNGetPlatformListByOSAndArchitecture(
			
			// const char* os
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string os  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}