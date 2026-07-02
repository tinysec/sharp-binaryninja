using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNMetadata* BNMetadataGetForKey(BNMetadata* data, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNMetadataGetForKey"
        )]
		internal static extern IntPtr BNMetadataGetForKey(
			
			// BNMetadata* data
		    IntPtr data  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  
		);
	}
}