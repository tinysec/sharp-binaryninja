using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNStringDetector* BNCreateStringDetector(const BNStringDetectionParameters* params)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateStringDetector"
        )]
		internal static extern IntPtr BNCreateStringDetector(
			
			// const BNStringDetectionParameters* params
		    IntPtr @params  
		);
	}
}
