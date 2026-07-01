using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetTypeBuilderFragmentStartBit(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeBuilderFragmentStartBit"
        )]
		internal static extern UIntPtr BNGetTypeBuilderFragmentStartBit(
			
			// BNTypeBuilder* type
		    IntPtr type  
		);
	}
}
