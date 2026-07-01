using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNValueLocationComponentToString(BNValueLocationComponent* component, BNArchitecture* arch)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNValueLocationComponentToString"
        )]
		internal static extern IntPtr BNValueLocationComponentToString(
			
			// BNValueLocationComponent* component
		    IntPtr component   , 
			
			// BNArchitecture* arch
		    IntPtr arch  
		);
	}
}
