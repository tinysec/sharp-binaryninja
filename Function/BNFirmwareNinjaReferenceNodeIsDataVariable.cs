using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFirmwareNinjaReferenceNodeIsDataVariable(BNFirmwareNinjaReferenceNode* node)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaReferenceNodeIsDataVariable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFirmwareNinjaReferenceNodeIsDataVariable(
			
			// BNFirmwareNinjaReferenceNode* node
		    IntPtr node  
			
		);
	}
}