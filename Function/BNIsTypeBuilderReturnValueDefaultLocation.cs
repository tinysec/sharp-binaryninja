using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNIsTypeBuilderReturnValueDefaultLocation(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsTypeBuilderReturnValueDefaultLocation"
        )]
		internal static extern bool BNIsTypeBuilderReturnValueDefaultLocation(
			
			// BNTypeBuilder* type
		    IntPtr type  
		);
	}
}
