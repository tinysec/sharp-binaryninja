using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNFirmwareNinjaRelationship* BNFirmwareNinjaGetRelationshipByGuid(BNFirmwareNinja* fn, const char* guid)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaGetRelationshipByGuid"
        )]
		internal static extern IntPtr BNFirmwareNinjaGetRelationshipByGuid(
			
			// BNFirmwareNinja* fn
		    IntPtr fn  , 
			
			// const char* guid
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string guid  
			
		);
	}
}