using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetTypeBuilderFragmentTruncatedStartBits(BNTypeBuilder* type, size_t truncatedBits)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetTypeBuilderFragmentTruncatedStartBits"
        )]
		internal static extern void BNSetTypeBuilderFragmentTruncatedStartBits(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// size_t truncatedBits
		    UIntPtr truncatedBits  
		);
	}
}
