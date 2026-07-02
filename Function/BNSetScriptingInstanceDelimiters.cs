using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetScriptingInstanceDelimiters(BNScriptingInstance* instance, const char* delimiters)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetScriptingInstanceDelimiters"
        )]
		internal static extern void BNSetScriptingInstanceDelimiters(
			
			// BNScriptingInstance* instance
		    IntPtr instance  , 
			
			// const char* delimiters
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string delimiters  
			
		);
	}
}