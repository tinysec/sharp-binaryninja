using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNRemoveTypeLibraryNamedType(BNTypeLibrary* lib, BNQualifiedName* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRemoveTypeLibraryNamedType"
        )]
		internal static extern void BNRemoveTypeLibraryNamedType(
			
			// BNTypeLibrary* lib
		    IntPtr lib   , 
			
			// BNQualifiedName* name
		    IntPtr name  
		);
	}
}
