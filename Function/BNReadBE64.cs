using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNReadBE64(BNBinaryReader* stream, uint64_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNReadBE64"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNReadBE64(
			
			// BNBinaryReader* stream
		    IntPtr stream  , 
			
			// uint64_t* result
		    out ulong result  
		);
	}
}