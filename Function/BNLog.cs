using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNLog( size_t session, BNLogLevel level, const char* logger_name, size_t tid, const char* fmt, ...)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLog"
        )]
		internal static extern void BNLog(
			
			// size_t session
		    UIntPtr session   , 
			
			// BNLogLevel level
		    LogLevel level   , 
			
			// const char* logger_name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string logger_name   , 
			
			// size_t tid
		    UIntPtr tid   , 
			
			// const char* fmt
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fmt   , 
			
			// ...
		    IntPtr arg5  
		);
	}
}
