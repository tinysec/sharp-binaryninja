using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetUserFunctionReturnValueLocation( BNFunction* func, BNValueLocationWithConfidence* location)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetUserFunctionReturnValueLocation"
        )]
		internal static extern void BNSetUserFunctionReturnValueLocation(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// BNValueLocationWithConfidence* location
		    IntPtr location  
		);
	}
}
