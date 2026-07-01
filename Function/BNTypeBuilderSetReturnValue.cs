using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNTypeBuilderSetReturnValue(BNTypeBuilder* type, BNReturnValue* rv)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNTypeBuilderSetReturnValue"
        )]
		internal static extern void BNTypeBuilderSetReturnValue(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// BNReturnValue* rv
		    IntPtr rv  
		);
	}
}
