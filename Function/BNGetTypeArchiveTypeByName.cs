using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNType* BNGetTypeArchiveTypeByName(BNTypeArchive* archive, BNQualifiedName* name, const char* snapshot)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeArchiveTypeByName"
        )]
		internal static extern IntPtr BNGetTypeArchiveTypeByName(
			
			// BNTypeArchive* archive
		    IntPtr archive  , 
			
			// BNQualifiedName* name
		    IntPtr name  , 
			
			// const char* snapshot
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string snapshot  
			
		);
	}
}