using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNPlatform* BNCreateCustomPlatformWithTypes(BNArchitecture* arch, const char* name, BNCustomPlatform* impl, const char* typeFile, const char** includeDirs, uint64_t includeDirCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateCustomPlatformWithTypes"
        )]
		internal static extern IntPtr BNCreateCustomPlatformWithTypes(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNCustomPlatform* impl
		    IntPtr impl  , 
			
			// const char* typeFile
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string typeFile  , 
			
			// const char** includeDirs
		    string[] includeDirs  , 
			
			// uint64_t includeDirCount
		    ulong includeDirCount  
		);
	}
}