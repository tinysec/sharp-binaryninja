using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNDeleteUserStackVariable(BNFunction* func, int64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDeleteUserStackVariable"
        )]
		internal static extern void BNDeleteUserStackVariable(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// int64_t offset
		    long offset  
			
		);
	}
}