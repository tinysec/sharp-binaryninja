using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFirmwareNinjaRelationshipGetSecondaryDataVariable(BNFirmwareNinjaRelationship* rel, BNDataVariable* dataVariable)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaRelationshipGetSecondaryDataVariable"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFirmwareNinjaRelationshipGetSecondaryDataVariable(
			
			// BNFirmwareNinjaRelationship* rel
		    IntPtr rel  , 
			
			// BNDataVariable* dataVariable
		    IntPtr dataVariable  
			
		);
	}
}