using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWrite8(BNBinaryWriter* stream, uint8_t val)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNWrite8"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWrite8(
			
			// BNBinaryWriter* stream
		    IntPtr stream  , 
			
			// uint8_t val
		    byte val  
		);
	}
}