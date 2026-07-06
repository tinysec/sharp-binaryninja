using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypeId(BNTypeContainer* container, BNQualifiedName* typeName, char** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeContainerGetTypeId"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerGetTypeId(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// BNQualifiedName* typeName
		    in BNQualifiedName typeName  , 
			
			// char** result
		    out IntPtr result  
		);
	}
}