using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNLinearViewObject* BNCreateLinearViewSingleFunctionLanguageRepresentation(BNFunction* func, BNDisassemblySettings* settings, const char* language)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNCreateLinearViewSingleFunctionLanguageRepresentation"
        )]
		internal static extern IntPtr BNCreateLinearViewSingleFunctionLanguageRepresentation(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// BNDisassemblySettings* settings
		    IntPtr settings  , 
			
			// const char* language
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string language  
		);
	}
}