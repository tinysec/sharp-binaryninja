using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetUserIsFunctionReturnValueDefaultLocation(BNFunction* func, bool defaultLocation)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetUserIsFunctionReturnValueDefaultLocation"
        )]
		internal static extern void BNSetUserIsFunctionReturnValueDefaultLocation(
			
			// BNFunction* func
		    IntPtr func   , 
			
			// bool defaultLocation
		    bool defaultLocation  
		);
	}
}
