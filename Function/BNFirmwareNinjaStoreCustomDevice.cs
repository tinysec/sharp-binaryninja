using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFirmwareNinjaStoreCustomDevice(BNFirmwareNinja* fn, const char* name, uint64_t start, uint64_t end, const char* info)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFirmwareNinjaStoreCustomDevice"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFirmwareNinjaStoreCustomDevice(
			
			// BNFirmwareNinja* fn
		    IntPtr fn  , 
			
			// const char* name
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string name  , 
			
			// uint64_t start
		    ulong start  , 
			
			// uint64_t end
		    ulong end  , 
			
			// const char* info
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string info  
			
		);
	}
}