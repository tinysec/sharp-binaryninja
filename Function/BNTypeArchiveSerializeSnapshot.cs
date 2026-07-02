using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNDataBuffer* BNTypeArchiveSerializeSnapshot(BNTypeArchive* archive, const char* snapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeArchiveSerializeSnapshot"
        )]
		internal static extern IntPtr BNTypeArchiveSerializeSnapshot(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* snapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string snapshot  
			
		);
	}
}