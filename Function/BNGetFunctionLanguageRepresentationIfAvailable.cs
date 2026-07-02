using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// BNLanguageRepresentationFunction* BNGetFunctionLanguageRepresentationIfAvailable(BNFunction* func, const char* language)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetFunctionLanguageRepresentationIfAvailable"
        )]
		internal static extern IntPtr BNGetFunctionLanguageRepresentationIfAvailable(
			
			// BNFunction* func
		    IntPtr func  , 
			
			// const char* language
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string language  
		);
	}
}