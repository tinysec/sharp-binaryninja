using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFunctionHasUserAnnotations(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFunctionHasUserAnnotations"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFunctionHasUserAnnotations(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}