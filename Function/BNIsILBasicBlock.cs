using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsILBasicBlock(BNBasicBlock* block)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsILBasicBlock"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsILBasicBlock(
			
			// BNBasicBlock* block
		    IntPtr block  
		);
	}
}