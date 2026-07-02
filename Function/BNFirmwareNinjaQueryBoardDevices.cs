using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// int32_t BNFirmwareNinjaQueryBoardDevices(BNFirmwareNinja* fn, BNArchitecture* arch, const char* board, BNFirmwareNinjaDevice** devices)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaQueryBoardDevices"
        )]
		internal static extern int BNFirmwareNinjaQueryBoardDevices(
			
			// BNFirmwareNinja* fn
		    IntPtr fn  , 
			
			// BNArchitecture* arch
		    IntPtr arch  , 
			
			// const char* board
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string board  , 
			
			// BNFirmwareNinjaDevice** devices
		    IntPtr devices  
			
		);
	}
}