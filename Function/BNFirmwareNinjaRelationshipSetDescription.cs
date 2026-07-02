using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNFirmwareNinjaRelationshipSetDescription(BNFirmwareNinjaRelationship* rel, const char* description)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaRelationshipSetDescription"
        )]
		internal static extern void BNFirmwareNinjaRelationshipSetDescription(
			
			// BNFirmwareNinjaRelationship* rel
		    IntPtr rel  , 
			
			// const char* description
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string description  
			
		);
	}
}