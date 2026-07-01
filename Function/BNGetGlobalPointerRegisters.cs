using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint32_t* BNGetGlobalPointerRegisters(BNCallingConvention* cc, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetGlobalPointerRegisters"
        )]
		internal static extern IntPtr BNGetGlobalPointerRegisters(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// size_t* count
		    out ulong count
		);
	}
}
