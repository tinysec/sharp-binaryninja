using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNEndianness BNGetTypeBuilderFragmentEndianness(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeBuilderFragmentEndianness"
        )]
		internal static extern Endianness BNGetTypeBuilderFragmentEndianness(
			
			// BNTypeBuilder* type
		    IntPtr type  
		);
	}
}
