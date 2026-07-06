using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDeleteSecretsProviderData(BNSecretsProvider* provider, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDeleteSecretsProviderData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNDeleteSecretsProviderData(
			
			// BNSecretsProvider* provider
		    IntPtr provider  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  
			
		);
	}
}