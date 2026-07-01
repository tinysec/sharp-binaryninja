using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetTypeBuilderFragmentStartBit(BNTypeBuilder* type, size_t startBit)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetTypeBuilderFragmentStartBit"
        )]
		internal static extern void BNSetTypeBuilderFragmentStartBit(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// size_t startBit
		    UIntPtr startBit  
		);
	}
}
