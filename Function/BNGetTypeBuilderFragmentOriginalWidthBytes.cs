using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetTypeBuilderFragmentOriginalWidthBytes(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeBuilderFragmentOriginalWidthBytes"
        )]
		internal static extern UIntPtr BNGetTypeBuilderFragmentOriginalWidthBytes(
			
			// BNTypeBuilder* type
		    IntPtr type  
		);
	}
}
