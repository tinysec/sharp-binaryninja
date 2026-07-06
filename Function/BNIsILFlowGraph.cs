using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsILFlowGraph(BNFlowGraph* graph)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsILFlowGraph"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsILFlowGraph(
			
			// BNFlowGraph* graph
		    IntPtr graph  
		);
	}
}