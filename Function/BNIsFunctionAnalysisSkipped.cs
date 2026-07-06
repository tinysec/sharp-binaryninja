using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsFunctionAnalysisSkipped(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsFunctionAnalysisSkipped"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsFunctionAnalysisSkipped(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}