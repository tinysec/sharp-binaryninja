using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNLoggerLogWithStackTrace( BNLogger* logger, BNLogLevel level, const char* stackTrace, const char* fmt, ...)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLoggerLogWithStackTrace"
        )]
		internal static extern void BNLoggerLogWithStackTrace(
			
			// BNLogger* logger
		    IntPtr logger   , 
			
			// BNLogLevel level
		    LogLevel level   , 
			
			// const char* stackTrace
		    string stackTrace   , 
			
			// const char* fmt
		    string fmt   , 
			
			// ...
		    IntPtr arg4  
		);
	}
}
