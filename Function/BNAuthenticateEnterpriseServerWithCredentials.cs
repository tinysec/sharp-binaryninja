using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAuthenticateEnterpriseServerWithCredentials(const char* username, const char* password, bool remember)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAuthenticateEnterpriseServerWithCredentials"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAuthenticateEnterpriseServerWithCredentials(
			
			// const char* username
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string username  , 
			
			// const char* password
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string password  , 
			
			// bool remember
		    bool remember  
		);
	}
}