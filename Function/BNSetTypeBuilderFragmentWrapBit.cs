using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetTypeBuilderFragmentWrapBit(BNTypeBuilder* type, size_t wrapBit)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetTypeBuilderFragmentWrapBit"
        )]
		internal static extern void BNSetTypeBuilderFragmentWrapBit(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// size_t wrapBit
		    UIntPtr wrapBit  
		);
	}
}
