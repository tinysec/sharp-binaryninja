using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNEndianness BNGetTypeFragmentEndianness(BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeFragmentEndianness"
        )]
		internal static extern Endianness BNGetTypeFragmentEndianness(
			
			// BNType* type
		    IntPtr type  
		);
	}
}
