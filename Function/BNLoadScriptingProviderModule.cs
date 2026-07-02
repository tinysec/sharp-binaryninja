using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNLoadScriptingProviderModule(BNScriptingProvider* provider, const char* repository, const char* module, bool force)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLoadScriptingProviderModule"
        )]
		internal static extern bool BNLoadScriptingProviderModule(
			
			// BNScriptingProvider* provider
		    IntPtr provider  , 
			
			// const char* repository
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string repository  , 
			
			// const char* module
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string module  , 
			
			// bool force
		    bool force  
			
		);
	}
}