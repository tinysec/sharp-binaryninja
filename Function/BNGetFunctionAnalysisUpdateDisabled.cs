using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetFunctionAnalysisUpdateDisabled(BNBinaryView* view)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetFunctionAnalysisUpdateDisabled"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetFunctionAnalysisUpdateDisabled(
			
			// BNBinaryView* view
		    IntPtr view  
		);
	}
}