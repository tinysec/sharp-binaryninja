using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNDeleteAutoStackVariable(BNFunction* func, int64_t offset)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDeleteAutoStackVariable"
        )]
		internal static extern void BNDeleteAutoStackVariable(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// int64_t offset
		    long offset  
			
		);
	}
}