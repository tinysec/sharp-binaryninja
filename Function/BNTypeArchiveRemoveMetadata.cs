using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeArchiveRemoveMetadata(BNTypeArchive* archive, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeArchiveRemoveMetadata"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeArchiveRemoveMetadata(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  
			
		);
	}
}