using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// double BNAnalysisContextGetSettingDouble(BNAnalysisContext* analysisContext, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisContextGetSettingDouble"
        )]
		internal static extern double BNAnalysisContextGetSettingDouble(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// const char* key
		    string key  
		);
	}
}
