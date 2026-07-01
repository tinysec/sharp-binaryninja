using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetAutoIsFunctionReturnValueDefaultLocation(BNFunction* func, bool defaultLocation)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetAutoIsFunctionReturnValueDefaultLocation"
        )]
		internal static extern void BNSetAutoIsFunctionReturnValueDefaultLocation(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// bool defaultLocation
		    bool defaultLocation  
		);
	}
}
