using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNScriptingInstanceCompleteArguments(BNScriptingInstance* instance, const char* text, uint64_t* argumentStart)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNScriptingInstanceCompleteArguments"
        )]
		internal static extern IntPtr BNScriptingInstanceCompleteArguments(
			
			// BNScriptingInstance* instance
		    IntPtr instance   , 
			
			// const char* text
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string text   , 
			
			// uint64_t* argumentStart
		    IntPtr argumentStart  
		);
	}
}
