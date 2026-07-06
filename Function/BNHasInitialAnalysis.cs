using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNHasInitialAnalysis(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNHasInitialAnalysis"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNHasInitialAnalysis(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}