using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNIsOffsetReadOnlySemantics(BNBinaryView* view, uint64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsOffsetReadOnlySemantics"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsOffsetReadOnlySemantics(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// uint64_t offset
		    ulong offset  
		);
	}
}
