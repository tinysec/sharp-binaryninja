using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFirmwareNinjaReferenceNodeGetDataVariable(BNFirmwareNinjaReferenceNode* node, BNDataVariable* dataVariable)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaReferenceNodeGetDataVariable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFirmwareNinjaReferenceNodeGetDataVariable(
			
			// BNFirmwareNinjaReferenceNode* node
		    IntPtr node  , 
			
			// BNDataVariable* dataVariable
		    IntPtr dataVariable  
			
		);
	}
}