using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNRegisterValueWithConfidenceAndRegister* BNGetUserGlobalPointerValues(BNBinaryView* view, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetUserGlobalPointerValues"
        )]
		internal static extern IntPtr BNGetUserGlobalPointerValues(
			
			// BNBinaryView* view
		    IntPtr view   , 
			
			// size_t* count
		    out ulong count
		);
	}
}
