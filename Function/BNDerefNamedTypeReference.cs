using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNType* BNDerefNamedTypeReference(BNBinaryView* view, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNDerefNamedTypeReference"
        )]
		internal static extern IntPtr BNDerefNamedTypeReference(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}
