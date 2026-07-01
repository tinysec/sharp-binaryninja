using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNCustomStringType** BNGetCustomStringTypeList(size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetCustomStringTypeList"
        )]
		internal static extern IntPtr BNGetCustomStringTypeList(
			
			// size_t* count
		    IntPtr count  
		);
	}
}
