using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNPlatform* BNCreatePlatformWithTypes(BNArchitecture* arch, const char* name, const char* typeFile, const char** includeDirs, uint64_t includeDirCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreatePlatformWithTypes"
        )]
		internal static extern IntPtr BNCreatePlatformWithTypes(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* typeFile
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeFile  , 
			
			// const char** includeDirs: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr includeDirs  , 
			
			// uint64_t includeDirCount
		    ulong includeDirCount  
		);
	}
}