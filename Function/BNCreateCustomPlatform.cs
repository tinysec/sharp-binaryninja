using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNPlatform* BNCreateCustomPlatform(BNArchitecture* arch, const char* name, BNCustomPlatform* impl)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateCustomPlatform"
        )]
		internal static extern IntPtr BNCreateCustomPlatform(
			IntPtr arch,
			[MarshalAs(UnmanagedType.LPUTF8Str)] string name,
			in BNCustomPlatform implementation
		);
	}
}
