using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNIsReturnTypeRegisterCompatible(BNCallingConvention* cc, BNBinaryView* view, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsReturnTypeRegisterCompatible"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsReturnTypeRegisterCompatible(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}
