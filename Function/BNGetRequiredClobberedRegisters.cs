using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// uint32_t* BNGetRequiredClobberedRegisters(BNCallingConvention* cc, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetRequiredClobberedRegisters"
        )]
		internal static extern IntPtr BNGetRequiredClobberedRegisters(
			
			// BNCallingConvention* cc
		    IntPtr cc   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
