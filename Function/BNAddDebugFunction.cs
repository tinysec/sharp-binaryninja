using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAddDebugFunction(BNDebugInfo* debugInfo, BNDebugFunctionInfo* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAddDebugFunction"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNAddDebugFunction(
			
			// BNDebugInfo* debugInfo
		    IntPtr debugInfo  , 
			
			// BNDebugFunctionInfo* func
		    IntPtr func  
		);
	}
}