using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNDefaultIsArgumentTypeRegisterCompatible(BNCallingConvention* cc, BNType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNDefaultIsArgumentTypeRegisterCompatible"
        )]
		internal static extern bool BNDefaultIsArgumentTypeRegisterCompatible(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// BNType* type
		    IntPtr type  
		);
	}
}
