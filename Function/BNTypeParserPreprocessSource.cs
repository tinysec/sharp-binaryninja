using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// bool BNTypeParserPreprocessSource(BNTypeParser* parser, const char* source, const char* fileName, BNPlatform* platform, BNTypeContainer* existingTypes, const char** options, uint64_t optionCount, const char** includeDirs, uint64_t includeDirCount, const char** output, BNTypeParserError** errors, uint64_t* errorCount)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNTypeParserPreprocessSource"
        )]
		[return: MarshalAs(UnmanagedType.I1)]
		internal static extern bool BNTypeParserPreprocessSource(
			
			// BNTypeParser* parser
		    IntPtr parser  , 
			
			// const char* source
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string source  , 
			
			// const char* fileName
		    [MarshalAs(UnmanagedType.LPUTF8Str)] string fileName  , 
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// BNTypeContainer* existingTypes
		    IntPtr existingTypes  , 
			
			// const char** options
		    IntPtr options  , 
			
			// uint64_t optionCount
		    ulong optionCount  , 
			
			// const char** includeDirs
		    IntPtr includeDirs  , 
			
			// uint64_t includeDirCount
		    ulong includeDirCount  , 
			
			// const char** output
		    out IntPtr output  , 
			
			// BNTypeParserError** errors
		    IntPtr errors  , 
			
			// uint64_t* errorCount
		    IntPtr errorCount  
			
		);
	}
}