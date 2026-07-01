using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNValueLocationWithConfidence BNGetFunctionReturnValueLocation(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetFunctionReturnValueLocation"
        )]
		internal static extern BNValueLocationWithConfidence BNGetFunctionReturnValueLocation(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}
