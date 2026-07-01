using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNCustomStringType* BNRegisterCustomStringType(BNCustomStringTypeInfo* info)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRegisterCustomStringType"
        )]
		internal static extern IntPtr BNRegisterCustomStringType(
			
			// BNCustomStringTypeInfo* info
		    IntPtr info  
		);
	}
}
