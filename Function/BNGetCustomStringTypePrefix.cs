using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNGetCustomStringTypePrefix(BNCustomStringType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetCustomStringTypePrefix"
        )]
		internal static extern IntPtr BNGetCustomStringTypePrefix(
			
			// BNCustomStringType* type
		    IntPtr type  
		);
	}
}
