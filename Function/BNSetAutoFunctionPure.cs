using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetAutoFunctionPure(BNFunction* func, BNBoolWithConfidence* pure)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetAutoFunctionPure"
        )]
		internal static extern void BNSetAutoFunctionPure(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNBoolWithConfidence* pure
		    IntPtr pure  
			
		);
	}
}