using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetUserFunctionParameterLocations( BNFunction* func, BNValueLocationListWithConfidence* locations)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetUserFunctionParameterLocations"
        )]
		internal static extern void BNSetUserFunctionParameterLocations(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// BNValueLocationListWithConfidence* locations
		    IntPtr locations  
		);
	}
}
