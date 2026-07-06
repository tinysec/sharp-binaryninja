using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFlowGraphHasNodes(BNFlowGraph* graph)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNFlowGraphHasNodes"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFlowGraphHasNodes(
			
			// BNFlowGraph* graph
		    IntPtr graph  
		);
	}
}