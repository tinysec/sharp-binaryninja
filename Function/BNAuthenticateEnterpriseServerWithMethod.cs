using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAuthenticateEnterpriseServerWithMethod(const char* method, bool remember)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAuthenticateEnterpriseServerWithMethod"
        )]
		internal static extern bool BNAuthenticateEnterpriseServerWithMethod(
			
			// const char* method
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string method  , 
			
			// bool remember
		    bool remember  
		);
	}
}