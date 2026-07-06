using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeLibraryDecompressToFile(const char* file, const char* output)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeLibraryDecompressToFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool BNTypeLibraryDecompressToFile(
			
			// const char* file
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string file  , 
			
			// const char* output
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string output  
		);
	}
}