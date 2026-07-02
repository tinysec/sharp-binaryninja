using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNRegisterPlatform(const char* os, BNPlatform* platform)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterPlatform"
        )]
		internal static extern void BNRegisterPlatform(
			
			// const char* os
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string os  , 
			
			// BNPlatform* platform
		    IntPtr platform  
		);
	}
}