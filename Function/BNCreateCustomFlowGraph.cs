using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNFlowGraph* BNCreateCustomFlowGraph(BNCustomFlowGraph* callbacks)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateCustomFlowGraph"
        )]
		internal static extern IntPtr BNCreateCustomFlowGraph(
			
			// BNCustomFlowGraph* callbacks
		    in BNCustomFlowGraph callbacks
		);
	}
}
