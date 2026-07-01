using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNValueLocationListWithConfidence BNGetFunctionParameterLocations(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetFunctionParameterLocations"
        )]
		internal static extern BNValueLocationListWithConfidence BNGetFunctionParameterLocations(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}
