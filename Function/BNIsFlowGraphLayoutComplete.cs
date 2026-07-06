using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsFlowGraphLayoutComplete(BNFlowGraph* graph)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsFlowGraphLayoutComplete"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsFlowGraphLayoutComplete(
			
			// BNFlowGraph* graph
		    IntPtr graph  
		);
	}
}