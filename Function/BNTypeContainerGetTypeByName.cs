using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypeByName(BNTypeContainer* container, BNQualifiedName* typeName, BNType** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeContainerGetTypeByName"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerGetTypeByName(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// BNQualifiedName* typeName
			in BNQualifiedName typeName  , 
			
			// BNType** result
		    out IntPtr result  
		);
	}
}