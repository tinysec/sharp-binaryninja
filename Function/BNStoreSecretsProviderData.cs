using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNStoreSecretsProviderData(BNSecretsProvider* provider, const char* key, const char* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNStoreSecretsProviderData"
        )]
		internal static extern bool BNStoreSecretsProviderData(
			
			// BNSecretsProvider* provider
		    IntPtr provider  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char* data
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string data  
			
		);
	}
}