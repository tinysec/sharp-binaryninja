using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWriteBE32(BNBinaryWriter* stream, uint32_t val)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNWriteBE32"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWriteBE32(
			
			// BNBinaryWriter* stream
		    IntPtr stream  , 
			
			// uint32_t val
		    uint val  
			
		);
	}
}