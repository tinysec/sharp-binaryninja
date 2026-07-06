using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsEndOfFile(BNBinaryReader* stream)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsEndOfFile"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsEndOfFile(
			
			// BNBinaryReader* stream
		    IntPtr stream  
		);
	}
}