using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNCustomStringType* BNGetCustomStringTypeByID(uint32_t id)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetCustomStringTypeByID"
        )]
		internal static extern IntPtr BNGetCustomStringTypeByID(
			
			// uint32_t id
		    uint id  
		);
	}
}
