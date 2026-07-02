using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNFlowGraph* BNCreateImmediateFunctionGraph(BNFunction* func, BNFunctionViewType type, BNDisassemblySettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateImmediateFunctionGraph"
        )]
		internal static extern IntPtr BNCreateImmediateFunctionGraph(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNFunctionViewType type
		    in BNFunctionViewType type  ,
			
			// BNDisassemblySettings* settings
		    IntPtr settings  
		);
	}
}