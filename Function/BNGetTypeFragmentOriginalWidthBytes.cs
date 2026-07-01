using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// size_t BNGetTypeFragmentOriginalWidthBytes(BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeFragmentOriginalWidthBytes"
        )]
		internal static extern UIntPtr BNGetTypeFragmentOriginalWidthBytes(
			
			// BNType* type
		    IntPtr type  
		);
	}
}
