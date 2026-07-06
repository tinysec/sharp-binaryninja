using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWrite16(BNBinaryWriter* stream, uint16_t val)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNWrite16"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWrite16(
			
			// BNBinaryWriter* stream
		    IntPtr stream  , 
			
			// uint16_t val
		    ushort val  
		);
	}
}