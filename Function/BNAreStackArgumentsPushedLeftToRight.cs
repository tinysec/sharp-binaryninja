using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNAreStackArgumentsPushedLeftToRight(BNCallingConvention* cc)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAreStackArgumentsPushedLeftToRight"
        )]
		internal static extern bool BNAreStackArgumentsPushedLeftToRight(
			
			// BNCallingConvention* cc
		    IntPtr cc  
		);
	}
}
