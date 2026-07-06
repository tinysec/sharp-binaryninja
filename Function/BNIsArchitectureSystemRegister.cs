using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsArchitectureSystemRegister(BNArchitecture* arch, uint32_t reg)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsArchitectureSystemRegister"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsArchitectureSystemRegister(
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// uint32_t reg
			RegisterIndex reg  
		);
	}
}