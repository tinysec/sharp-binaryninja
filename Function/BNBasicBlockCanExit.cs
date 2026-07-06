using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBasicBlockCanExit(BNBasicBlock* block)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBasicBlockCanExit"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNBasicBlockCanExit(
			
			// BNBasicBlock* block
		    IntPtr block  
		);
	}
}