using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetActiveUpdateChannel(const char* channel)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetActiveUpdateChannel"
        )]
		internal static extern void BNSetActiveUpdateChannel(
			
			// const char* channel
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string channel  
			
		);
	}
}