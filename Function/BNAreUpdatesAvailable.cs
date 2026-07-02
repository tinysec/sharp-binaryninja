using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAreUpdatesAvailable(const char* channel, uint64_t* expireTime, uint64_t* serverTime, const char** errors)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAreUpdatesAvailable"
        )]
		internal static extern bool BNAreUpdatesAvailable(
			
			// const char* channel
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string channel  , 
			
			// uint64_t* expireTime
		    out ulong expireTime  , 
			
			// uint64_t* serverTime
		    out ulong serverTime  , 
			
			// const char** errors
		    out IntPtr errors  
		);
	}
}