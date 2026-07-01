using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetAutoFunctionCallingConvention(BNFunction* func, BNCallingConventionWithConfidence* convention)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetAutoFunctionCallingConvention"
        )]
		internal static extern void BNSetAutoFunctionCallingConvention(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNCallingConventionWithConfidence* convention
		    IntPtr convention  
			
		);
	}
}