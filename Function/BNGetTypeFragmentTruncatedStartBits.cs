using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetTypeFragmentTruncatedStartBits(BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeFragmentTruncatedStartBits"
        )]
		internal static extern UIntPtr BNGetTypeFragmentTruncatedStartBits(
			
			// BNType* type
		    IntPtr type  
		);
	}
}
