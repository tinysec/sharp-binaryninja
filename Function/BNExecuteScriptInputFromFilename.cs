using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNScriptingProviderExecuteResult BNExecuteScriptInputFromFilename(BNScriptingInstance* instance, const char* filename)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNExecuteScriptInputFromFilename"
        )]
		internal static extern ScriptingProviderExecuteResult BNExecuteScriptInputFromFilename(
			
			// BNScriptingInstance* instance
		    IntPtr instance  , 
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename  
			
		);
	}
}