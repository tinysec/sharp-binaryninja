using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNGetTypeParserOptionText(BNTypeParser* parser, BNTypeParserOption option, const char* value, const char** result)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNGetTypeParserOptionText"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNGetTypeParserOptionText(
			
			// BNTypeParser* parser
		    IntPtr parser  , 
			
			// BNTypeParserOption option
		    TypeParserOption option  , 
			
			// const char* _value
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string _value  , 
			
			// const char** result
		    out IntPtr result  
			
		);
	}
}