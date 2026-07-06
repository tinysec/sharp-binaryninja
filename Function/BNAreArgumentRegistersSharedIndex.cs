using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAreArgumentRegistersSharedIndex(BNCallingConvention* cc)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAreArgumentRegistersSharedIndex"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAreArgumentRegistersSharedIndex(
			
			// BNCallingConvention* cc
		    IntPtr cc  
		);
	}
}