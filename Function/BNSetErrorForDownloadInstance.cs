using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetErrorForDownloadInstance(BNDownloadInstance* instance, const char* error)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetErrorForDownloadInstance"
        )]
		internal static extern void BNSetErrorForDownloadInstance(
			
			// BNDownloadInstance* instance
		    IntPtr instance  , 
			
			// const char* error
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string error  
			
		);
	}
}