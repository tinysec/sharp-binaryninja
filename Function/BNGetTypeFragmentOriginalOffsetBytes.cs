using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNGetTypeFragmentOriginalOffsetBytes(BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeFragmentOriginalOffsetBytes"
        )]
		internal static extern ulong BNGetTypeFragmentOriginalOffsetBytes(
			
			// BNType* type
		    IntPtr type  
		);
	}
}
