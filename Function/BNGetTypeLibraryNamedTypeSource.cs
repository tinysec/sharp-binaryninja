using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNGetTypeLibraryNamedTypeSource(BNTypeLibrary* lib, BNQualifiedName* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeLibraryNamedTypeSource"
        )]
		internal static extern IntPtr BNGetTypeLibraryNamedTypeSource(
			
			// BNTypeLibrary* lib
		    IntPtr lib   , 
			
			// BNQualifiedName* name
		    IntPtr name  
		);
	}
}
