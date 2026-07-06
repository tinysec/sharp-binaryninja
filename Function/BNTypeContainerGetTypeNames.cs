using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypeNames(BNTypeContainer* container, BNQualifiedName** typeNames, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeContainerGetTypeNames"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerGetTypeNames(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// BNQualifiedName** typeNames
		    out IntPtr typeNames  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}