using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetFunctionComment(BNFunction* func, const char* comment)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetFunctionComment"
        )]
		internal static extern void BNSetFunctionComment(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// const char* comment
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string comment  
		);
	}
}