using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNStringRecognizer* BNRegisterStringRecognizer( const char* name, BNCustomStringRecognizer* recognizer)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNRegisterStringRecognizer"
        )]
		internal static extern IntPtr BNRegisterStringRecognizer(
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name   , 
			
			// BNCustomStringRecognizer* recognizer
		    IntPtr recognizer  
		);
	}
}
