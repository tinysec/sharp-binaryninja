using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNAnalysisContextInform(BNAnalysisContext* analysisContext, const char* request)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisContextInform"
        )]
		internal static extern bool BNAnalysisContextInform(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext  , 
			
			// const char* request
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string request  
		);
	}
}