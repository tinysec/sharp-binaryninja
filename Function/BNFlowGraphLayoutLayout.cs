using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNFlowGraphLayoutLayout(BNFlowGraphLayout* layout, BNFlowGraph* graph, BNFlowGraphNode** nodes, uint64_t nodeCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNFlowGraphLayoutLayout"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNFlowGraphLayoutLayout(
			
			// BNFlowGraphLayout* layout
		    IntPtr layout  , 
			
			// BNFlowGraph* graph
		    IntPtr graph  , 
			
			// BNFlowGraphNode** nodes
		    IntPtr nodes  , 
			
			// uint64_t nodeCount
		    ulong nodeCount  
			
		);
	}
}