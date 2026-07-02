using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNFirmwareNinjaRelationshipSetProvenance(BNFirmwareNinjaRelationship* rel, const char* provenance)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaRelationshipSetProvenance"
        )]
		internal static extern void BNFirmwareNinjaRelationshipSetProvenance(
			
			// BNFirmwareNinjaRelationship* rel
		    IntPtr rel  , 
			
			// const char* provenance
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string provenance  
			
		);
	}
}