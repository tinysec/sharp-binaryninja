using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNNotifyErrorForScriptingInstance(BNScriptingInstance* instance, const char* text)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNNotifyErrorForScriptingInstance"
        )]
		internal static extern void BNNotifyErrorForScriptingInstance(
			
			// BNScriptingInstance* instance
		    IntPtr instance  , 
			
			// const char* text
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string text  
			
		);
	}
}