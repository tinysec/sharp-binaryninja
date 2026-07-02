using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNScriptingProvider* BNRegisterScriptingProvider(const char* name, const char* apiName, BNScriptingProviderCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterScriptingProvider"
        )]
		internal static extern IntPtr BNRegisterScriptingProvider(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// const char* apiName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string apiName  , 
			
			// BNScriptingProviderCallbacks* callbacks
		    IntPtr callbacks  
			
		);
	}
}