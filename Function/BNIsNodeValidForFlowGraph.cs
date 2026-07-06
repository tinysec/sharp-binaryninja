using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsNodeValidForFlowGraph(BNFlowGraph* graph, BNFlowGraphNode* node)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsNodeValidForFlowGraph"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsNodeValidForFlowGraph(
			
			// BNFlowGraph* graph
		    IntPtr graph  , 
			
			// BNFlowGraphNode* node
		    IntPtr node  
		);
	}
}