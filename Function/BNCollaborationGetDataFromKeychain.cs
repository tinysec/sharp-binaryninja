using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// uint64_t BNCollaborationGetDataFromKeychain(const char* key, const char*** foundKeys, const char*** foundValues)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCollaborationGetDataFromKeychain"
        )]
		internal static extern ulong BNCollaborationGetDataFromKeychain(
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// const char*** foundKeys
		    IntPtr foundKeys  , 
			
			// const char*** foundValues
		    IntPtr foundValues  
		);
	}
}