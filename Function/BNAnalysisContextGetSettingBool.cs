using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// bool BNAnalysisContextGetSettingBool(BNAnalysisContext* analysisContext, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisContextGetSettingBool"
        )]
		internal static extern bool BNAnalysisContextGetSettingBool(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// const char* key
		    string key  
		);
	}
}
