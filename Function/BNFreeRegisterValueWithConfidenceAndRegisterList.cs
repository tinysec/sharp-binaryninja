using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeRegisterValueWithConfidenceAndRegisterList(BNRegisterValueWithConfidenceAndRegister* values)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeRegisterValueWithConfidenceAndRegisterList"
        )]
		internal static extern void BNFreeRegisterValueWithConfidenceAndRegisterList(
			
			// BNRegisterValueWithConfidenceAndRegister* values
		    IntPtr values  
		);
	}
}
