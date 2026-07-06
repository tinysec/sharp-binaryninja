using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNWriteData(BNBinaryWriter* stream, void* src, uint64_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNWriteData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNWriteData(
			
			// BNBinaryWriter* stream
		    IntPtr stream  , 
			
			// void* src
		    byte[] src  , 
			
			// uint64_t len
		    ulong len  
		);
	}
}