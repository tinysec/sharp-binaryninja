using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWrite64(BNBinaryWriter* stream, uint64_t val)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNWrite64"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWrite64(
			
			// BNBinaryWriter* stream
		    IntPtr stream  , 
			
			// uint64_t val
		    ulong val  
		);
	}
}