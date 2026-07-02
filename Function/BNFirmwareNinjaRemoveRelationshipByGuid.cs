using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNFirmwareNinjaRemoveRelationshipByGuid(BNFirmwareNinja* fn, const char* guid)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaRemoveRelationshipByGuid"
        )]
		internal static extern void BNFirmwareNinjaRemoveRelationshipByGuid(
			
			// BNFirmwareNinja* fn
		    IntPtr fn  , 
			
			// const char* guid
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string guid  
			
		);
	}
}