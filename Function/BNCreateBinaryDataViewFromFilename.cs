using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNBinaryView* BNCreateBinaryDataViewFromFilename(BNFileMetadata* file, const char* filename)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateBinaryDataViewFromFilename"
        )]
		internal static extern IntPtr BNCreateBinaryDataViewFromFilename(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// const char* filename
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string filename  
		);
	}
}