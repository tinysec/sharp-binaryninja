using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNUpdateResult BNUpdateToVersion(const char* channel, const char* version, const char** errors, void** progress, void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUpdateToVersion"
        )]
		internal static extern UpdateResult BNUpdateToVersion(
			
			// const char* channel
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string channel  , 
			
			// const char* version
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string version  , 
			
			// const char** errors
		    out IntPtr errors  ,
			
			// void** progress
		    IntPtr progress  , 
			
			// void* context
		    IntPtr context  
			
		);
	}
}
