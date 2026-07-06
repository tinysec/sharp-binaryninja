using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewLookupImportedTypeLibrary(BNBinaryView* view, BNQualifiedName* typeName, BNTypeLibrary** lib, BNQualifiedName* resultName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBinaryViewLookupImportedTypeLibrary"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNBinaryViewLookupImportedTypeLibrary(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNQualifiedName* typeName
		    in BNQualifiedName typeName  , 
			
			// BNTypeLibrary** lib
		    out IntPtr lib  , 
			
			// BNQualifiedName* resultName
			out BNQualifiedName resultName  
		);
	}
}