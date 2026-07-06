using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsArchitectureGlobalRegister(BNArchitecture* arch, uint32_t reg)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsArchitectureGlobalRegister"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsArchitectureGlobalRegister(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint32_t reg
			RegisterIndex reg  
		);
	}
}