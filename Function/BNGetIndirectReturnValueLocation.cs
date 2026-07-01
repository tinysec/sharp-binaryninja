using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNVariable BNGetIndirectReturnValueLocation(BNCallingConvention* cc)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetIndirectReturnValueLocation"
        )]
		internal static extern BNVariable BNGetIndirectReturnValueLocation(
			
			// BNCallingConvention* cc
		    IntPtr cc  
		);
	}
}
