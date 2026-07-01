using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNValueLocationWithConfidence BNGetTypeBuilderReturnValueLocation(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeBuilderReturnValueLocation"
        )]
		internal static extern BNValueLocationWithConfidence BNGetTypeBuilderReturnValueLocation(
			
			// BNTypeBuilder* type
		    IntPtr type  
		);
	}
}
