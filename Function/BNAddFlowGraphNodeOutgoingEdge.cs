using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNAddFlowGraphNodeOutgoingEdge(BNFlowGraphNode* node, BNBranchType type, BNFlowGraphNode* target, BNEdgeStyle edgeStyle)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNAddFlowGraphNodeOutgoingEdge"
        )]
		internal static extern void BNAddFlowGraphNodeOutgoingEdge(
			
			// BNFlowGraphNode* node
		    IntPtr node  , 
			
			// BNBranchType type
		    BranchType type  , 
			
			// BNFlowGraphNode* target
		    IntPtr target  , 
			
			// BNEdgeStyle edgeStyle
		    BNEdgeStyle edgeStyle
		);
	}
}
