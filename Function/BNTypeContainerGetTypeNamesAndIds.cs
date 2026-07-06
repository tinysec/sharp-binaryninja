using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypeNamesAndIds(BNTypeContainer* container, const char*** typeIds, BNQualifiedName** typeNames, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeContainerGetTypeNamesAndIds"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerGetTypeNamesAndIds(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// const char*** typeIds
			out IntPtr typeIds  , 
			
			// BNQualifiedName** typeNames
			out IntPtr typeNames  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}