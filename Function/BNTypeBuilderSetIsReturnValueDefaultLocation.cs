using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNTypeBuilderSetIsReturnValueDefaultLocation(BNTypeBuilder* type, bool defaultLocation)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeBuilderSetIsReturnValueDefaultLocation"
        )]
		internal static extern void BNTypeBuilderSetIsReturnValueDefaultLocation(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// bool defaultLocation
		    bool defaultLocation  
		);
	}
}
