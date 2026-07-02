using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNLogger* BNLogCreateLogger(const char* loggerName, uint64_t sessionId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLogCreateLogger"
        )]
		internal static extern IntPtr BNLogCreateLogger(
			
			// const char* loggerName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string loggerName  , 
			
			// uint64_t sessionId
		    ulong sessionId  
		);
	}
}