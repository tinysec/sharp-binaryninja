using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWrite32(BNBinaryWriter* stream, uint32_t val)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNWrite32"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWrite32(
			
			// BNBinaryWriter* stream
		    IntPtr stream  , 
			
			// uint32_t val
		    uint val  
		);
	}
}