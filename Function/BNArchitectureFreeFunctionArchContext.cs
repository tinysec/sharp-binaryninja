using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNArchitectureFreeFunctionArchContext(BNArchitecture* arch, void* context)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNArchitectureFreeFunctionArchContext"
        )]
		internal static extern void BNArchitectureFreeFunctionArchContext(
			
			// BNArchitecture* arch
		    IntPtr arch   , 
			
			// void* context
		    IntPtr context  
		);
	}
}
