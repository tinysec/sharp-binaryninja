using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDemangler* BNRegisterDemangler(const char* name, BNDemanglerCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterDemangler"
        )]
		internal static extern IntPtr BNRegisterDemangler(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNDemanglerCallbacks* callbacks
		    IntPtr callbacks  
			
		);
	}
}