using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// BNInlineDuringAnalysisWithConfidence BNGetFunctionInlinedDuringAnalysis(BNFunction* func)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            EntryPoint = "BNGetFunctionInlinedDuringAnalysis"
        )]
		internal static extern BNInlineDuringAnalysisWithConfidence BNGetFunctionInlinedDuringAnalysis(
			
			// BNFunction* func
		    IntPtr func  
		);
	}
}
