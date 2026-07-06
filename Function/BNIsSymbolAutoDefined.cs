using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsSymbolAutoDefined(BNSymbol* sym)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsSymbolAutoDefined"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsSymbolAutoDefined(
			
			// BNSymbol* sym
		    IntPtr sym  
		);
	}
}