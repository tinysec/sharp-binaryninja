using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNFlowGraph* BNCreateFunctionGraph(BNFunction* func, BNFunctionViewType type, BNDisassemblySettings* settings)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNCreateFunctionGraph"
        )]
		internal static extern IntPtr BNCreateFunctionGraph(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNFunctionViewType type
		    in BNFunctionViewType type  ,
			
			// BNDisassemblySettings* settings
		    IntPtr settings  
		);
	}
}