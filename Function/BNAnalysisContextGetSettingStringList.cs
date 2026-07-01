using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {

	    /// <summary>
		/// char** BNAnalysisContextGetSettingStringList(BNAnalysisContext* analysisContext, const char* key, size_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNAnalysisContextGetSettingStringList"
        )]
		internal static extern IntPtr BNAnalysisContextGetSettingStringList(
			
			// BNAnalysisContext* analysisContext
		    IntPtr analysisContext   , 
			
			// const char* key
		    string key   , 
			
			// size_t* count
		    IntPtr count  
		);
	}
}
