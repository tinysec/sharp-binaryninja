using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetAutoFunctionCanReturn(BNFunction* func, BNBoolWithConfidence* returns)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetAutoFunctionCanReturn"
        )]
		internal static extern void BNSetAutoFunctionCanReturn(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNBoolWithConfidence* returns
		    IntPtr returns  
			
		);
	}
}