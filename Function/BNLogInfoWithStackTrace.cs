using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNLogInfoWithStackTrace(const char* stackTrace, const char* fmt, ...)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLogInfoWithStackTrace"
        )]
		internal static extern void BNLogInfoWithStackTrace(
			
			// const char* stackTrace
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string stackTrace   , 
			
			// const char* fmt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt   , 
			
			// ...
		    IntPtr arg2  
		);
	}
}
