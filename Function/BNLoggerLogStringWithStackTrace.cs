using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNLoggerLogStringWithStackTrace(BNLogger* logger, BNLogLevel level, const char* stackTrace, const char* msg)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLoggerLogStringWithStackTrace"
        )]
		internal static extern void BNLoggerLogStringWithStackTrace(
			
			// BNLogger* logger
		    IntPtr logger  , 
			
			// BNLogLevel level
		    LogLevel level  , 
			
			// const char* stackTrace
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string stackTrace  , 
			
			// const char* msg
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string msg  
			
		);
	}
}