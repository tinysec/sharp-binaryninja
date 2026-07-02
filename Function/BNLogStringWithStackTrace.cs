using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNLogStringWithStackTrace(uint64_t session, BNLogLevel level, const char* logger_name, uint64_t tid, const char* stackTrace, const char* str)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLogStringWithStackTrace"
        )]
		internal static extern void BNLogStringWithStackTrace(
			
			// uint64_t session
		    ulong session  , 
			
			// BNLogLevel level
		    LogLevel level  , 
			
			// const char* logger_name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string logger_name  , 
			
			// uint64_t tid
		    ulong tid  , 
			
			// const char* stackTrace
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string stackTrace  , 
			
			// const char* str
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string str  
			
		);
	}
}