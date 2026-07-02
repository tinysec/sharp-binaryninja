using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNLoggerLog(BNLogger* logger, BNLogLevel level, const char* fmt, ...)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLoggerLog"
        )]
		internal static extern void BNLoggerLog(
			
			// BNLogger* logger
		    IntPtr logger   , 
			
			// BNLogLevel level
		    LogLevel level   , 
			
			// const char* fmt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt   , 
			
			// ...
		    IntPtr arg3  
		);
	}
}
