using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddTypeLibraryNamedTypeSource(BNTypeLibrary* lib, BNQualifiedName* name, const char* source)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAddTypeLibraryNamedTypeSource"
        )]
		internal static extern void BNAddTypeLibraryNamedTypeSource(
			
			// BNTypeLibrary* lib
		    IntPtr lib  , 
			
			// BNQualifiedName* name
		    in BNQualifiedName name  , 
			
			// const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  
		);
	}
}