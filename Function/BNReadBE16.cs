using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNReadBE16(BNBinaryReader* stream, uint16_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNReadBE16"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNReadBE16(
			
			// BNBinaryReader* stream
		    IntPtr stream  , 
			
			// uint16_t* result
		    out ushort result  
		);
	}
}