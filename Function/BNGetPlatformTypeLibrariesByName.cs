using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeLibrary** BNGetPlatformTypeLibrariesByName(BNPlatform* platform, const char* depName, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetPlatformTypeLibrariesByName"
        )]
		internal static extern IntPtr BNGetPlatformTypeLibrariesByName(
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// const char* depName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string depName  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}