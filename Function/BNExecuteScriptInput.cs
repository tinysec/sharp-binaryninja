using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNScriptingProviderExecuteResult BNExecuteScriptInput(BNScriptingInstance* instance, const char* input)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNExecuteScriptInput"
        )]
		internal static extern ScriptingProviderExecuteResult BNExecuteScriptInput(
			
			// BNScriptingInstance* instance
		    IntPtr instance  , 
			
			// const char* input
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string input  
			
		);
	}
}