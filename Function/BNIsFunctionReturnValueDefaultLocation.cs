using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNIsFunctionReturnValueDefaultLocation(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsFunctionReturnValueDefaultLocation"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsFunctionReturnValueDefaultLocation(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}
