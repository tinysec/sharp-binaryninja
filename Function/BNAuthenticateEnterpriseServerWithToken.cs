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
		internal static extern bool BNAuthenticateEnterpriseServerWithToken(
			
			// const char* token
		    string token   , 
			
			// bool remember
		    bool remember  
		);
	}
}
