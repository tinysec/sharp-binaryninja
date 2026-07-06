using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNInstallScriptingProviderModules(BNScriptingProvider* provider, const char* modules)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNInstallScriptingProviderModules"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNInstallScriptingProviderModules(
			
			// BNScriptingProvider* provider
		    IntPtr provider  , 
			
			// const char* modules
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string modules  
			
		);
	}
}