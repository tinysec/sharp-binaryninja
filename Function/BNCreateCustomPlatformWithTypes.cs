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
			IntPtr arch,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string name,
			in BNCustomPlatform implementation,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string typeFile,
			IntPtr includeDirs,
			ulong includeDirCount
		);
	}
}
