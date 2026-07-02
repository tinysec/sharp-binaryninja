using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNUpdateResult BNUpdateToLatestVersion(const char* channel, const char** errors, void** progress, void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUpdateToLatestVersion"
        )]
		internal static extern UpdateResult BNUpdateToLatestVersion(
			
			// const char* channel
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string channel  , 
			
			// const char** errors
		    string[] errors  , 
			
			// void** progress
		    IntPtr progress  , 
			
			// void* context
		    IntPtr context  
			
		);
	}
}