using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNIsFlowGraphOptionSet(BNFlowGraph* graph, BNFlowGraphOption option)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNIsFlowGraphOptionSet"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNIsFlowGraphOptionSet(
			
			// BNFlowGraph* graph
		    IntPtr graph  , 
			
			// BNFlowGraphOption option
		    FlowGraphOption option  
		);
	}
}