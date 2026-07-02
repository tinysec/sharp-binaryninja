using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNLineFormatter* BNRegisterLineFormatter(const char* name, BNCustomLineFormatter* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterLineFormatter"
        )]
		internal static extern IntPtr BNRegisterLineFormatter(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// BNCustomLineFormatter* callbacks
		    IntPtr callbacks  
			
		);
	}
}