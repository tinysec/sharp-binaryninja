using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace BinaryNinja
{
    internal static partial class NativeMethods
    {
	    /// <summary>
		/// void BNPlatformAdjustTypeParserInput(BNPlatform* platform, BNTypeParser* parser, const char** argumentsIn, uint64_t argumentsLenIn, const char** sourceFileNamesIn, const char** sourceFileValuesIn, uint64_t sourceFilesLenIn, const char*** argumentsOut, uint64_t* argumentsLenOut, const char*** sourceFileNamesOut, const char*** sourceFileValuesOut, uint64_t* sourceFilesLenOut)
		/// </summary>
		[DllImport(
            "binaryninjacore", 
            CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl,
            CharSet = CharSet.Ansi,
            EntryPoint = "BNPlatformAdjustTypeParserInput"
        )]
		internal static extern void BNPlatformAdjustTypeParserInput(
			
			// BNPlatform* platform
		    IntPtr platform  , 
			
			// BNTypeParser* parser
		    IntPtr parser  , 
			
			// const char** argumentsIn: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr argumentsIn  , 
			
			// uint64_t argumentsLenIn
		    ulong argumentsLenIn  , 
			
			// const char** sourceFileNamesIn: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr sourceFileNamesIn  ,

			// const char** sourceFileValuesIn: caller-built UTF-8 char** block (string[]
			// elements cannot carry LPUTF8Str, so the wrapper builds the block).
		    IntPtr sourceFileValuesIn  , 
			
			// uint64_t sourceFilesLenIn
		    ulong sourceFilesLenIn  , 
			
			// const char*** argumentsOut
		    IntPtr argumentsOut  , 
			
			// uint64_t* argumentsLenOut
		    IntPtr argumentsLenOut  , 
			
			// const char*** sourceFileNamesOut
		    IntPtr sourceFileNamesOut  , 
			
			// const char*** sourceFileValuesOut
		    IntPtr sourceFileValuesOut  , 
			
			// uint64_t* sourceFilesLenOut
		    IntPtr sourceFilesLenOut  
			
		);
	}
}