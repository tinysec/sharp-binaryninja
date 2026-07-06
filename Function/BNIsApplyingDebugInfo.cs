using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsApplyingDebugInfo(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNIsApplyingDebugInfo"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsApplyingDebugInfo(
			
			// BNBinaryView* view
		    IntPtr view  
			
		);
	}
}