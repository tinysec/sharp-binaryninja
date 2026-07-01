using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNRemoveTypeLibraryNamedObject(BNTypeLibrary* lib, BNQualifiedName* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRemoveTypeLibraryNamedObject"
        )]
		internal static extern void BNRemoveTypeLibraryNamedObject(
			
			// BNTypeLibrary* lib
		    IntPtr lib   , 
			
			// BNQualifiedName* name
		    IntPtr name  
		);
	}
}
