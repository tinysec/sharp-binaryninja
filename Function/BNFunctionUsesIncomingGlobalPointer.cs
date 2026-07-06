using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFunctionUsesIncomingGlobalPointer(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFunctionUsesIncomingGlobalPointer"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFunctionUsesIncomingGlobalPointer(
			
			// BNFunction* func
		    IntPtr func  
			
		);
	}
}