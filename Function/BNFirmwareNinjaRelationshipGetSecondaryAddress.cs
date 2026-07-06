using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFirmwareNinjaRelationshipGetSecondaryAddress(BNFirmwareNinjaRelationship* rel, uint64_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaRelationshipGetSecondaryAddress"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFirmwareNinjaRelationshipGetSecondaryAddress(
			
			// BNFirmwareNinjaRelationship* rel
		    IntPtr rel  , 
			
			// uint64_t* result
		    IntPtr result  
			
		);
	}
}