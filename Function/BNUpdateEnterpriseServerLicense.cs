using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNUpdateEnterpriseServerLicense(uint64_t timeout)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNUpdateEnterpriseServerLicense"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNUpdateEnterpriseServerLicense(
			
			// uint64_t timeout
		    ulong timeout  
			
		);
	}
}