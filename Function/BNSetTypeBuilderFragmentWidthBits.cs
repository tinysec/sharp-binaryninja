using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetTypeBuilderFragmentWidthBits(BNTypeBuilder* type, size_t widthBits)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetTypeBuilderFragmentWidthBits"
        )]
		internal static extern void BNSetTypeBuilderFragmentWidthBits(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// size_t widthBits
		    UIntPtr widthBits  
		);
	}
}
