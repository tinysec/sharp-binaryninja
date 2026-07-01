using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNValueLocationToString(BNValueLocation* location, BNArchitecture* arch)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNValueLocationToString"
        )]
		internal static extern IntPtr BNValueLocationToString(
			
			// BNValueLocation* location
		    IntPtr location   , 
			
			// BNArchitecture* arch
		    IntPtr arch  
		);
	}
}
