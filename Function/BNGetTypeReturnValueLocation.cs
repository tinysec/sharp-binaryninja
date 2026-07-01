using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNValueLocationWithConfidence BNGetTypeReturnValueLocation(BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetTypeReturnValueLocation"
        )]
		internal static extern BNValueLocationWithConfidence BNGetTypeReturnValueLocation(
			
			// BNType* type
		    IntPtr type  
		);
	}
}
