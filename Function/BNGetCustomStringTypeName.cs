using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNGetCustomStringTypeName(BNCustomStringType* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetCustomStringTypeName"
        )]
		internal static extern IntPtr BNGetCustomStringTypeName(
			
			// BNCustomStringType* type
		    IntPtr type  
		);
	}
}
