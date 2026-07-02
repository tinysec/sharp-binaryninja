using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNTypeArchive* BNLookupTypeArchiveById(const char* id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNLookupTypeArchiveById"
        )]
		internal static extern IntPtr BNLookupTypeArchiveById(
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  
			
		);
	}
}