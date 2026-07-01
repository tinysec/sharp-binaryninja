using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// void BNFreeParameterLocations(BNValueLocationListWithConfidence* locations)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFreeParameterLocations"
        )]
		internal static extern void BNFreeParameterLocations(
			
			// BNValueLocationListWithConfidence* locations
		    IntPtr locations  
		);
	}
}
