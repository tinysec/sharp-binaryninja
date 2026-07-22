using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNSecretsProvider* BNRegisterSecretsProvider(const char* name, BNSecretsProviderCallbacks* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterSecretsProvider"
        )]
		internal static extern IntPtr BNRegisterSecretsProvider(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNSecretsProviderCallbacks* callbacks
		    in BNSecretsProviderCallbacks callbacks
			
		);
	}
}
