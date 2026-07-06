using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSecretsProviderHasData(BNSecretsProvider* provider, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSecretsProviderHasData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNSecretsProviderHasData(
			
			// BNSecretsProvider* provider
		    IntPtr provider  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  
			
		);
	}
}