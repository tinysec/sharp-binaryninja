using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNArchitectureSetDefaultAnalyzeBasicBlocksCallback(void* callback)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNArchitectureSetDefaultAnalyzeBasicBlocksCallback"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNArchitectureSetDefaultAnalyzeBasicBlocksCallback(
			
			// void* callback
		    IntPtr callback  
		);
	}
}