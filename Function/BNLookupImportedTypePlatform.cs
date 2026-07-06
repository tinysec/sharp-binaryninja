using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNLookupImportedTypePlatform(BNBinaryView* view, BNQualifiedName* typeName, BNPlatform** platform, BNQualifiedName* resultName)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNLookupImportedTypePlatform"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNLookupImportedTypePlatform(
			
			// BNBinaryView* view
		    IntPtr view  , 
			
			// BNQualifiedName* typeName
		    in BNQualifiedName typeName  , 
			
			// BNPlatform** platform
		    out IntPtr platform  , 
			
			// BNQualifiedName* resultName
			out BNQualifiedName resultName  
		);
	}
}