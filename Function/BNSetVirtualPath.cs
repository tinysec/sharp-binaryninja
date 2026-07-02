using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetVirtualPath(BNFileMetadata* file, const char* path)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetVirtualPath"
        )]
		internal static extern void BNSetVirtualPath(
			
			// BNFileMetadata* file
		    IntPtr file  , 
			
			// const char* path
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string path  
			
		);
	}
}