using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetTypeBuilderFragmentEndianness(BNTypeBuilder* type, BNEndianness endianness)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetTypeBuilderFragmentEndianness"
        )]
		internal static extern void BNSetTypeBuilderFragmentEndianness(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// BNEndianness endianness
		    Endianness endianness  
		);
	}
}
