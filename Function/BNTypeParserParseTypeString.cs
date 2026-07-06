using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeParserParseTypeString(BNTypeParser* parser, const char* source, BNPlatform* platform, BNTypeContainer* existingTypes, BNQualifiedNameAndType* result, BNTypeParserError** errors, uint64_t* errorCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeParserParseTypeString"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeParserParseTypeString(
			
			// BNTypeParser* parser
		    IntPtr parser  , 
			
			// const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  , 
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// BNTypeContainer* existingTypes
		    IntPtr existingTypes  , 
			
			// BNQualifiedNameAndType* result
		    IntPtr result  , 
			
			// BNTypeParserError** errors
		    IntPtr errors  , 
			
			// uint64_t* errorCount
		    IntPtr errorCount  
			
		);
	}
}