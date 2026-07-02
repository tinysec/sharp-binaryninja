using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNType* BNGetTypeArchiveTypeById(BNTypeArchive* archive, const char* id, const char* snapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeArchiveTypeById"
        )]
		internal static extern IntPtr BNGetTypeArchiveTypeById(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* id
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string id  , 
			
			// const char* snapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string snapshot  
			
		);
	}
}