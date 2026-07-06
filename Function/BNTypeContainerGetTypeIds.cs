using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeContainerGetTypeIds(BNTypeContainer* container, const char*** typeIds, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeContainerGetTypeIds"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeContainerGetTypeIds(
			
			// BNTypeContainer* container
		    IntPtr container  , 
			
			// char*** typeIds
		    out IntPtr typeIds  , 
			
			// uint64_t* count
		    out ulong count  
		);
	}
}