using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNRebase(BNBinaryView* data, uint64_t address)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNRebase"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNRebase(
			
			// BNBinaryView* data
		    IntPtr data  , 
			
			// uint64_t address
		    ulong address  
		);
	}
}