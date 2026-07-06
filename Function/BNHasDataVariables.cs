using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNHasDataVariables(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHasDataVariables"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNHasDataVariables(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}