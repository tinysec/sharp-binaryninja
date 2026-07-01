using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetUserFunctionReturnValue(BNFunction* func, BNReturnValue* returnValue)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetUserFunctionReturnValue"
        )]
		internal static extern void BNSetUserFunctionReturnValue(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// BNReturnValue* returnValue
		    IntPtr returnValue  
		);
	}
}
