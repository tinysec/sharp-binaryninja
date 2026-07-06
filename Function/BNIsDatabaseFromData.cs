using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsDatabaseFromData(void* data, uint64_t len)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsDatabaseFromData"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsDatabaseFromData(
			
			// void* data
		    IntPtr data  , 
			
			// uint64_t len
		    ulong len  
			
		);
	}
}