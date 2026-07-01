using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNGetCustomStringTypePostfix(BNCustomStringType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetCustomStringTypePostfix"
        )]
		internal static extern IntPtr BNGetCustomStringTypePostfix(
			
			// BNCustomStringType* type
		    IntPtr type  
		);
	}
}
