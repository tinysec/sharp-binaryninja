using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNTypeBuilderSetReturnValueLocation( BNTypeBuilder* type, BNValueLocationWithConfidence* location)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeBuilderSetReturnValueLocation"
        )]
		internal static extern void BNTypeBuilderSetReturnValueLocation(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// BNValueLocationWithConfidence* location
		    IntPtr location  
		);
	}
}
