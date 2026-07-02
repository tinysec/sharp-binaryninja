using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNSetEnterpriseServerUrl(const char* url)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetEnterpriseServerUrl"
        )]
		internal static extern bool BNSetEnterpriseServerUrl(
			
			// const char* url
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string url  
			
		);
	}
}