using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNRegisterValueWithConfidenceAndRegister* BNGetFunctionGlobalPointerValues(BNFunction* func, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetFunctionGlobalPointerValues"
        )]
		internal static extern IntPtr BNGetFunctionGlobalPointerValues(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// size_t* count
		    out ulong count
		);
	}
}
