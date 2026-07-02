using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBinaryView* BNOpenExistingDatabase(BNFileMetadata* file, const char* path)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNOpenExistingDatabase"
        )]
		internal static extern IntPtr BNOpenExistingDatabase(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  
		);
	}
}