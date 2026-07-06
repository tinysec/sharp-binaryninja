using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRead8(BNBinaryReader* stream, uint8_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRead8"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRead8(
			
			// BNBinaryReader* stream
		    IntPtr stream  , 
			
			// uint8_t* result
		    out byte result  
		);
	}
}