using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNMetadataSetValueForKey(BNMetadata* data, const char* key, BNMetadata* md)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNMetadataSetValueForKey"
        )]
		internal static extern bool BNMetadataSetValueForKey(
			
			// BNMetadata* data
		    IntPtr data  , 
			
			// const char* key
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string key  , 
			
			// BNMetadata* md
		    IntPtr md  
			
		);
	}
}