using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// const char** BNParseTypeParserOptionsText(const char* optionsText, uint64_t* count)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNParseTypeParserOptionsText"
        )]
		internal static extern IntPtr BNParseTypeParserOptionsText(
			
			// const char* optionsText
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string optionsText  , 
			
			// uint64_t* count
		    IntPtr count  
			
		);
	}
}