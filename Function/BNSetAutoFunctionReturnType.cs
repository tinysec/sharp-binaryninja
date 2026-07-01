using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetAutoFunctionReturnType(BNFunction* func, BNTypeWithConfidence* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetAutoFunctionReturnType"
        )]
		internal static extern void BNSetAutoFunctionReturnType(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNTypeWithConfidence* type
		    IntPtr type  
			
		);
	}
}