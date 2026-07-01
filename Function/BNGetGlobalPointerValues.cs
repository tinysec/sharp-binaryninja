using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNRegisterValueWithConfidenceAndRegister* BNGetGlobalPointerValues(BNBinaryView* view, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetGlobalPointerValues"
        )]
		internal static extern IntPtr BNGetGlobalPointerValues(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// size_t* count
		    out ulong count
		);
	}
}
