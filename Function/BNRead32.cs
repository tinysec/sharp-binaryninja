using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRead32(BNBinaryReader* stream, uint32_t* result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRead32"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRead32(
			
			// BNBinaryReader* stream
		    IntPtr stream  , 
			
			// uint32_t* result
		    out uint result  
		);
	}
}