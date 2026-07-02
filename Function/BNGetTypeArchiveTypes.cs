using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNQualifiedNameTypeAndId* BNGetTypeArchiveTypes(BNTypeArchive* archive, const char* snapshot, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeArchiveTypes"
        )]
		internal static extern IntPtr BNGetTypeArchiveTypes(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// const char* snapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string snapshot  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}