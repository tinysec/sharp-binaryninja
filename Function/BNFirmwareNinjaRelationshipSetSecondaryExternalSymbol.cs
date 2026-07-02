using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNFirmwareNinjaRelationshipSetSecondaryExternalSymbol(BNFirmwareNinjaRelationship* rel, BNProjectFile* projectFile, const char* symbol)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaRelationshipSetSecondaryExternalSymbol"
        )]
		internal static extern void BNFirmwareNinjaRelationshipSetSecondaryExternalSymbol(
			
			// BNFirmwareNinjaRelationship* rel
		    IntPtr rel  , 
			
			// BNProjectFile* projectFile
		    IntPtr projectFile  , 
			
			// const char* symbol
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string symbol  
			
		);
	}
}