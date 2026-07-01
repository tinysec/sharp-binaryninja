using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetTypeFragmentStartBit(BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeFragmentStartBit"
        )]
		internal static extern UIntPtr BNGetTypeFragmentStartBit(
			
			// BNType* type
		    IntPtr type  
		);
	}
}
