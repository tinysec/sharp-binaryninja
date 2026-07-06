using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsTypeBuilderFloatingPoint(BNTypeBuilder* type)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsTypeBuilderFloatingPoint"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsTypeBuilderFloatingPoint(
			
			// BNTypeBuilder* type
		    IntPtr type  
			
		);
	}
}