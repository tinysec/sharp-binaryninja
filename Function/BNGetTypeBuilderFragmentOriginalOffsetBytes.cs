using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint64_t BNGetTypeBuilderFragmentOriginalOffsetBytes(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeBuilderFragmentOriginalOffsetBytes"
        )]
		internal static extern ulong BNGetTypeBuilderFragmentOriginalOffsetBytes(
			
			// BNTypeBuilder* type
		    IntPtr type  
		);
	}
}
