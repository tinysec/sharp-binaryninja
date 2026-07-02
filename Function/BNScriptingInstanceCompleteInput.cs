using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char* BNScriptingInstanceCompleteInput(BNScriptingInstance* instance, const char* text, uint64_t state)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNScriptingInstanceCompleteInput"
        )]
		internal static extern IntPtr BNScriptingInstanceCompleteInput(
			
			// BNScriptingInstance* instance
		    IntPtr instance  , 
			
			// const char* text
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string text  , 
			
			// uint64_t state
		    ulong state  
			
		);
	}
}