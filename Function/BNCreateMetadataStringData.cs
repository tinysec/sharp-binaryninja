using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNMetadata* BNCreateMetadataStringData(const char* data)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateMetadataStringData"
        )]
		internal static extern IntPtr BNCreateMetadataStringData(
			
			// const char* data
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string data  
		);
	}
}