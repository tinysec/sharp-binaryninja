using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNSetUserFunctionInlinedDuringAnalysis(BNFunction* func, BNInlineDuringAnalysisWithConfidence inlined)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNSetUserFunctionInlinedDuringAnalysis"
        )]
		internal static extern void BNSetUserFunctionInlinedDuringAnalysis(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNInlineDuringAnalysisWithConfidence inlined
		    BNInlineDuringAnalysisWithConfidence inlined
			
		);
	}
}
