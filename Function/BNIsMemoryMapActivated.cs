using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsMemoryMapActivated(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsMemoryMapActivated"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsMemoryMapActivated(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}