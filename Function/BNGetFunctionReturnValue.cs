using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNReturnValue BNGetFunctionReturnValue(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetFunctionReturnValue"
        )]
		internal static extern BNReturnValue BNGetFunctionReturnValue(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}
