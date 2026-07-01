using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetTypeBuilderFragmentOriginalWidthBytes(BNTypeBuilder* type, size_t size)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetTypeBuilderFragmentOriginalWidthBytes"
        )]
		internal static extern void BNSetTypeBuilderFragmentOriginalWidthBytes(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// size_t size
		    UIntPtr size  
		);
	}
}
