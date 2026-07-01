using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetEnumerationBuilder(BNTypeBuilder* type, BNEnumerationBuilder* e)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetEnumerationBuilder"
        )]
		internal static extern void BNSetEnumerationBuilder(
			
			// BNTypeBuilder* type
		    IntPtr type   , 
			
			// BNEnumerationBuilder* e
		    IntPtr e  
		);
	}
}
