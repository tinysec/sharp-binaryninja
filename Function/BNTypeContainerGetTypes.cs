using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypes(BNTypeContainer* container, const char*** typeIds, BNQualifiedName** typeNames, BNType*** types, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeContainerGetTypes"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerGetTypes(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// char*** typeIds
		    out IntPtr typeIds  , 
			
			// BNQualifiedName** typeNames
			out IntPtr typeNames  , 
			
			// BNType*** types
			out IntPtr types  , 
			
			// uint64_t* count
			out ulong count  
		);
	}
}