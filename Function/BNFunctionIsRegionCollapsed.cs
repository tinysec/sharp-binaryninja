using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFunctionIsRegionCollapsed(BNFunction* func, uint64_t hash)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFunctionIsRegionCollapsed"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFunctionIsRegionCollapsed(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// uint64_t hash
		    ulong hash  
			
		);
	}
}