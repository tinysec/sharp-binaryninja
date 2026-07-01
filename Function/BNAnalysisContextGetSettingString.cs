using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char* BNAnalysisContextGetSettingString(BNAnalysisContext* analysisContext, const char* key)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisContextGetSettingString"
        )]
		internal static extern IntPtr BNAnalysisContextGetSettingString(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// const char* key
		    string key  
		);
	}
}
