using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFunctionHasUserType(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFunctionHasUserType"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFunctionHasUserType(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}