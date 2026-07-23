using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNCallingConvention* BNCreateCallingConvention(BNArchitecture* arch, const char* name, BNCustomCallingConvention* cc)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateCallingConvention"
        )]
		internal static extern IntPtr BNCreateCallingConvention(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNCustomCallingConvention* cc
		    in BNCustomCallingConvention convention
		);
	}
}
