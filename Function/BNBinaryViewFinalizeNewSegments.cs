using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNBinaryViewFinalizeNewSegments(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNBinaryViewFinalizeNewSegments"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNBinaryViewFinalizeNewSegments(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}