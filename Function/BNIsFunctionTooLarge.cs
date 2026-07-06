using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsFunctionTooLarge(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsFunctionTooLarge"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsFunctionTooLarge(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}