using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNDetectBaseAddress(BNBaseAddressDetection* bad, BNBaseAddressDetectionSettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNDetectBaseAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNDetectBaseAddress(
			
			// BNBaseAddressDetection* bad
		    IntPtr bad  , 
			
			// BNBaseAddressDetectionSettings* settings
		    IntPtr settings  
			
		);
	}
}