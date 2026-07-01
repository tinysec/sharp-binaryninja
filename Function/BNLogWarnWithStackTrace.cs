using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNLogWarnWithStackTrace(const char* stackTrace, const char* fmt, ...)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLogWarnWithStackTrace"
        )]
		internal static extern void BNLogWarnWithStackTrace(
			
			// const char* stackTrace
		    string stackTrace   , 
			
			// const char* fmt
		    string fmt   , 
			
			// ...
		    IntPtr arg2  
		);
	}
}
