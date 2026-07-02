using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNRegisterValueWithConfidenceAndRegister* BNGetDefaultGlobalPointerValues(BNBinaryView* view, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetDefaultGlobalPointerValues"
        )]
		internal static extern IntPtr BNGetDefaultGlobalPointerValues(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// size_t* count
		    out ulong count
		);
	}
}
