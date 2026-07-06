using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNReadData(BNBinaryReader* stream, void* dest, uint64_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNReadData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNReadData(
			
			// BNBinaryReader* stream
		    IntPtr stream  , 
			
			// void* dest
		    byte[] dest  , 
			
			// uint64_t len
		    ulong len  
		);
	}
}