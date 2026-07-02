using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNSettings* BNCreateSettings(const char* schemaId)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateSettings"
        )]
		internal static extern IntPtr BNCreateSettings(
			
			// const char* schemaId
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string schemaId  
		);
	}
}