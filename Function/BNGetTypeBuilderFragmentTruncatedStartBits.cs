using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetTypeBuilderFragmentTruncatedStartBits(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeBuilderFragmentTruncatedStartBits"
        )]
		internal static extern UIntPtr BNGetTypeBuilderFragmentTruncatedStartBits(
			
			// BNTypeBuilder* type
		    IntPtr type  
		);
	}
}
