using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNCopyFile(const char* source, const char* dest)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCopyFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNCopyFile(
			
			// const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  , 
			
			// const char* dest
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string dest  
		);
	}
}