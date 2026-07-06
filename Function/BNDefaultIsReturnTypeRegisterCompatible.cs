using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNDefaultIsReturnTypeRegisterCompatible(BNCallingConvention* cc, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNDefaultIsReturnTypeRegisterCompatible"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNDefaultIsReturnTypeRegisterCompatible(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}
