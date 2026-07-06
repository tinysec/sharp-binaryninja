using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNAuthenticateEnterpriseServerWithToken( const char* token, bool remember)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAuthenticateEnterpriseServerWithToken"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAuthenticateEnterpriseServerWithToken(
			
			// const char* token
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string token   , 
			
			// bool remember
		    bool remember  
		);
	}
}
