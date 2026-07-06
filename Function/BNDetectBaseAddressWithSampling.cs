using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNDetectBaseAddressWithSampling(BNBaseAddressDetection* bad, BNBaseAddressDetectionSamplingSettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNDetectBaseAddressWithSampling"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNDetectBaseAddressWithSampling(
			
			// BNBaseAddressDetection* bad
		    IntPtr bad   , 
			
			// BNBaseAddressDetectionSamplingSettings* settings
		    IntPtr settings  
		);
	}
}
