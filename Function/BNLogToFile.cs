using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNLogToFile(BNLogLevel minimumLevel, const char* path, bool append)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLogToFile"
        )]
		public static extern bool BNLogToFile(
			
			// BNLogLevel minimumLevel
		    LogLevel minimumLevel  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  , 
			
			// bool append
		    bool append  
		);
	}
}