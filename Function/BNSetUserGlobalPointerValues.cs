using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNSetUserGlobalPointerValues(BNBinaryView* view, const BNRegisterValueWithConfidenceAndRegister* values, size_t count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNSetUserGlobalPointerValues"
        )]
		internal static extern void BNSetUserGlobalPointerValues(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// const BNRegisterValueWithConfidenceAndRegister* values
		    IntPtr values   , 
			
			// size_t count
		    UIntPtr count  
		);
	}
}
