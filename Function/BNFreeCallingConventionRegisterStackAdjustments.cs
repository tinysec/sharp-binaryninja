using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeCallingConventionRegisterStackAdjustments(uint32_t* regs, int32_t* adjust)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeCallingConventionRegisterStackAdjustments"
        )]
		internal static extern void BNFreeCallingConventionRegisterStackAdjustments(
			
			// uint32_t* regs
		    IntPtr regs   , 
			
			// int32_t* adjust
		    IntPtr adjust  
		);
	}
}
