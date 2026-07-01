using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeReturnValue(BNReturnValue* ret)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeReturnValue"
        )]
		internal static extern void BNFreeReturnValue(
			
			// BNReturnValue* ret
		    IntPtr ret  
		);
	}
}
