using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNWebsocketProvider** BNGetWebsocketProviderList(uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetWebsocketProviderList"
        )]
		internal static extern IntPtr BNGetWebsocketProviderList(
			
			// uint64_t* count
		    out UIntPtr count
			
		);
	}
}
