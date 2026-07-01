using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetNamedTypeReferenceBuilder(BNTypeBuilder* type, BNNamedTypeReferenceBuilder* nt)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetNamedTypeReferenceBuilder"
        )]
		internal static extern void BNSetNamedTypeReferenceBuilder(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// BNNamedTypeReferenceBuilder* nt
		    IntPtr nt  
		);
	}
}
