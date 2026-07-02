using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNLoggerLogString(BNLogger* logger, BNLogLevel level, const char* msg)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLoggerLogString"
        )]
		internal static extern void BNLoggerLogString(
			
			// BNLogger* logger
		    IntPtr logger  , 
			
			// BNLogLevel level
		    LogLevel level  , 
			
			// const char* msg
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string msg  
		);
	}
}