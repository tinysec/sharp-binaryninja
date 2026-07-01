using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetAutoFunctionReturnValueLocation( BNFunction* func, BNValueLocationWithConfidence* location)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetAutoFunctionReturnValueLocation"
        )]
		internal static extern void BNSetAutoFunctionReturnValueLocation(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// BNValueLocationWithConfidence* location
		    IntPtr location  
		);
	}
}
