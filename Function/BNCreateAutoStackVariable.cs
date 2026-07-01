using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNCreateAutoStackVariable(BNFunction* func, int64_t offset, BNTypeWithConfidence* type, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateAutoStackVariable"
        )]
		internal static extern void BNCreateAutoStackVariable(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// int64_t offset
		    long offset  , 
			
			// BNTypeWithConfidence* type
		    IntPtr type  , 
			
			// const char* name
		    string name  
		);
	}
}