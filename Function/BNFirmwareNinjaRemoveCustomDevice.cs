using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFirmwareNinjaRemoveCustomDevice(BNFirmwareNinja* fn, const char* name)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaRemoveCustomDevice"
        )]
		internal static extern bool BNFirmwareNinjaRemoveCustomDevice(
			
			// BNFirmwareNinja* fn
		    IntPtr fn  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  
			
		);
	}
}